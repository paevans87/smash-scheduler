CREATE TABLE subscriptions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    club_id UUID NOT NULL REFERENCES clubs(id) ON DELETE CASCADE,
    status TEXT NOT NULL DEFAULT 'trialling',
    plan_type TEXT NOT NULL DEFAULT 'free',
    current_period_start TIMESTAMPTZ NOT NULL DEFAULT now(),
    current_period_end TIMESTAMPTZ,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT chk_subscriptions_status CHECK (status IN ('active', 'trialling', 'cancelled', 'expired')),
    CONSTRAINT uq_subscriptions_club_id UNIQUE (club_id)
);

CREATE INDEX idx_subscriptions_club_id ON subscriptions (club_id);
CREATE INDEX idx_subscriptions_status ON subscriptions (status);

ALTER TABLE subscriptions ENABLE ROW LEVEL SECURITY;

CREATE POLICY subscriptions_policy ON subscriptions
    FOR ALL
    TO authenticated
    USING (
        club_id IN (SELECT club_id FROM club_organisers WHERE user_id = auth.uid())
    )
    WITH CHECK (
        club_id IN (SELECT club_id FROM club_organisers WHERE user_id = auth.uid())
    );

CREATE TRIGGER trg_subscriptions_updated_at
    BEFORE UPDATE ON subscriptions
    FOR EACH ROW
    EXECUTE FUNCTION set_updated_at();
