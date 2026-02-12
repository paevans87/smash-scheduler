CREATE TABLE clubs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES auth.users(id) ON DELETE CASCADE,
    name TEXT NOT NULL,
    default_court_count INT NOT NULL DEFAULT 1,
    game_type SMALLINT NOT NULL DEFAULT 1,
    blacklist_mode SMALLINT NOT NULL DEFAULT 0,
    scoring_weight_skill_balance INT NOT NULL DEFAULT 40,
    scoring_weight_match_history INT NOT NULL DEFAULT 35,
    scoring_weight_time_off_court INT NOT NULL DEFAULT 25,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT chk_clubs_game_type CHECK (game_type IN (0, 1)),
    CONSTRAINT chk_clubs_blacklist_mode CHECK (blacklist_mode IN (0, 1)),
    CONSTRAINT chk_clubs_scoring_weights_sum CHECK (
        scoring_weight_skill_balance + scoring_weight_match_history + scoring_weight_time_off_court = 100
    ),
    CONSTRAINT chk_clubs_scoring_weights_non_negative CHECK (
        scoring_weight_skill_balance >= 0
        AND scoring_weight_match_history >= 0
        AND scoring_weight_time_off_court >= 0
    )
);

CREATE TABLE players (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    club_id UUID NOT NULL REFERENCES clubs(id) ON DELETE CASCADE,
    name TEXT NOT NULL,
    skill_level INT NOT NULL DEFAULT 5,
    gender SMALLINT NOT NULL DEFAULT 0,
    play_style_preference SMALLINT NOT NULL DEFAULT 2,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT chk_players_skill_level CHECK (skill_level BETWEEN 1 AND 10),
    CONSTRAINT chk_players_gender CHECK (gender IN (0, 1, 2)),
    CONSTRAINT chk_players_play_style_preference CHECK (play_style_preference IN (0, 1, 2))
);

CREATE TABLE sessions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    club_id UUID NOT NULL REFERENCES clubs(id) ON DELETE CASCADE,
    scheduled_date_time TIMESTAMPTZ NOT NULL,
    court_count INT NOT NULL DEFAULT 1,
    state SMALLINT NOT NULL DEFAULT 0,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT chk_sessions_state CHECK (state IN (0, 1, 2)),
    CONSTRAINT chk_sessions_court_count CHECK (court_count >= 1)
);

CREATE TABLE session_court_labels (
    session_id UUID NOT NULL REFERENCES sessions(id) ON DELETE CASCADE,
    court_number INT NOT NULL,
    label TEXT NOT NULL,

    PRIMARY KEY (session_id, court_number),
    CONSTRAINT chk_session_court_labels_court_number CHECK (court_number >= 1)
);

CREATE TABLE session_players (
    session_id UUID NOT NULL REFERENCES sessions(id) ON DELETE CASCADE,
    player_id UUID NOT NULL REFERENCES players(id) ON DELETE CASCADE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    joined_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    PRIMARY KEY (session_id, player_id)
);

CREATE TABLE matches (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    session_id UUID NOT NULL REFERENCES sessions(id) ON DELETE CASCADE,
    court_number INT NOT NULL,
    state SMALLINT NOT NULL DEFAULT 0,
    was_automated BOOLEAN NOT NULL DEFAULT FALSE,
    started_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    completed_at TIMESTAMPTZ,
    team1_score INT,
    team2_score INT,
    winning_team SMALLINT,

    CONSTRAINT chk_matches_state CHECK (state IN (0, 1)),
    CONSTRAINT chk_matches_court_number CHECK (court_number >= 1),
    CONSTRAINT chk_matches_winning_team CHECK (winning_team IS NULL OR winning_team IN (1, 2)),
    CONSTRAINT chk_matches_scores_both_or_neither CHECK (
        (team1_score IS NULL AND team2_score IS NULL)
        OR (team1_score IS NOT NULL AND team2_score IS NOT NULL)
    ),
    CONSTRAINT chk_matches_scores_non_negative CHECK (
        (team1_score IS NULL OR team1_score >= 0)
        AND (team2_score IS NULL OR team2_score >= 0)
    )
);

CREATE TABLE match_players (
    match_id UUID NOT NULL REFERENCES matches(id) ON DELETE CASCADE,
    player_id UUID NOT NULL REFERENCES players(id) ON DELETE CASCADE,
    team_number SMALLINT NOT NULL,

    PRIMARY KEY (match_id, player_id),
    CONSTRAINT chk_match_players_team_number CHECK (team_number IN (1, 2))
);

CREATE TABLE player_blacklists (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id UUID NOT NULL REFERENCES players(id) ON DELETE CASCADE,
    blacklisted_player_id UUID NOT NULL REFERENCES players(id) ON DELETE CASCADE,
    blacklist_type SMALLINT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT chk_player_blacklists_type CHECK (blacklist_type IN (0, 1)),
    CONSTRAINT chk_player_blacklists_no_self CHECK (player_id != blacklisted_player_id),
    CONSTRAINT uq_player_blacklists UNIQUE (player_id, blacklisted_player_id, blacklist_type)
);
