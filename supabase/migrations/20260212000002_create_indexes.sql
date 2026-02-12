CREATE INDEX idx_clubs_user_id ON clubs(user_id);

CREATE INDEX idx_players_club_id ON players(club_id);

CREATE INDEX idx_sessions_club_id ON sessions(club_id);

CREATE INDEX idx_session_players_player_id ON session_players(player_id);

CREATE INDEX idx_matches_session_id ON matches(session_id);

CREATE INDEX idx_match_players_player_id ON match_players(player_id);

CREATE INDEX idx_player_blacklists_player_id ON player_blacklists(player_id);
CREATE INDEX idx_player_blacklists_blacklisted_player_id ON player_blacklists(blacklisted_player_id);
