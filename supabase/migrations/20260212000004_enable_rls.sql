ALTER TABLE clubs ENABLE ROW LEVEL SECURITY;
ALTER TABLE players ENABLE ROW LEVEL SECURITY;
ALTER TABLE sessions ENABLE ROW LEVEL SECURITY;
ALTER TABLE session_court_labels ENABLE ROW LEVEL SECURITY;
ALTER TABLE session_players ENABLE ROW LEVEL SECURITY;
ALTER TABLE matches ENABLE ROW LEVEL SECURITY;
ALTER TABLE match_players ENABLE ROW LEVEL SECURITY;
ALTER TABLE player_blacklists ENABLE ROW LEVEL SECURITY;

CREATE POLICY clubs_policy ON clubs
    FOR ALL
    TO authenticated
    USING (user_id = auth.uid())
    WITH CHECK (user_id = auth.uid());

CREATE POLICY players_policy ON players
    FOR ALL
    TO authenticated
    USING (
        club_id IN (SELECT id FROM clubs WHERE user_id = auth.uid())
    )
    WITH CHECK (
        club_id IN (SELECT id FROM clubs WHERE user_id = auth.uid())
    );

CREATE POLICY sessions_policy ON sessions
    FOR ALL
    TO authenticated
    USING (
        club_id IN (SELECT id FROM clubs WHERE user_id = auth.uid())
    )
    WITH CHECK (
        club_id IN (SELECT id FROM clubs WHERE user_id = auth.uid())
    );

CREATE POLICY session_court_labels_policy ON session_court_labels
    FOR ALL
    TO authenticated
    USING (
        session_id IN (
            SELECT s.id FROM sessions s
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    )
    WITH CHECK (
        session_id IN (
            SELECT s.id FROM sessions s
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    );

CREATE POLICY session_players_policy ON session_players
    FOR ALL
    TO authenticated
    USING (
        session_id IN (
            SELECT s.id FROM sessions s
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    )
    WITH CHECK (
        session_id IN (
            SELECT s.id FROM sessions s
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    );

CREATE POLICY matches_policy ON matches
    FOR ALL
    TO authenticated
    USING (
        session_id IN (
            SELECT s.id FROM sessions s
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    )
    WITH CHECK (
        session_id IN (
            SELECT s.id FROM sessions s
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    );

CREATE POLICY match_players_policy ON match_players
    FOR ALL
    TO authenticated
    USING (
        match_id IN (
            SELECT m.id FROM matches m
            JOIN sessions s ON s.id = m.session_id
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    )
    WITH CHECK (
        match_id IN (
            SELECT m.id FROM matches m
            JOIN sessions s ON s.id = m.session_id
            JOIN clubs c ON c.id = s.club_id
            WHERE c.user_id = auth.uid()
        )
    );

CREATE POLICY player_blacklists_policy ON player_blacklists
    FOR ALL
    TO authenticated
    USING (
        player_id IN (
            SELECT p.id FROM players p
            JOIN clubs c ON c.id = p.club_id
            WHERE c.user_id = auth.uid()
        )
    )
    WITH CHECK (
        player_id IN (
            SELECT p.id FROM players p
            JOIN clubs c ON c.id = p.club_id
            WHERE c.user_id = auth.uid()
        )
    );
