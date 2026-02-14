ALTER TABLE clubs ADD COLUMN slug TEXT;
ALTER TABLE sessions ADD COLUMN slug TEXT;

CREATE OR REPLACE FUNCTION generate_club_slug(p_name TEXT)
RETURNS TEXT
LANGUAGE plpgsql
AS $$
DECLARE
  v_base TEXT;
  v_slug TEXT;
  v_suffix INT := 1;
BEGIN
  v_base := trim(both '-' from regexp_replace(lower(p_name), '[^a-z0-9]+', '-', 'g'));

  IF v_base = '' THEN
    v_base := 'club';
  END IF;

  v_slug := v_base;

  WHILE EXISTS (SELECT 1 FROM clubs WHERE slug = v_slug) LOOP
    v_suffix := v_suffix + 1;
    v_slug := v_base || '-' || v_suffix;
  END LOOP;

  RETURN v_slug;
END;
$$;

CREATE OR REPLACE FUNCTION generate_session_slug(p_club_id UUID, p_date TIMESTAMPTZ)
RETURNS TEXT
LANGUAGE plpgsql
AS $$
DECLARE
  v_base TEXT;
  v_slug TEXT;
  v_suffix INT := 1;
BEGIN
  v_base := to_char(p_date, 'YYYY-MM-DD');
  v_slug := v_base;

  WHILE EXISTS (SELECT 1 FROM sessions WHERE club_id = p_club_id AND slug = v_slug) LOOP
    v_suffix := v_suffix + 1;
    v_slug := v_base || '-' || v_suffix;
  END LOOP;

  RETURN v_slug;
END;
$$;

UPDATE clubs SET slug = generate_club_slug(name) WHERE slug IS NULL;
UPDATE sessions SET slug = generate_session_slug(club_id, scheduled_date_time) WHERE slug IS NULL;

ALTER TABLE clubs ALTER COLUMN slug SET NOT NULL;
ALTER TABLE clubs ADD CONSTRAINT uq_clubs_slug UNIQUE (slug);
CREATE INDEX idx_clubs_slug ON clubs (slug);

ALTER TABLE sessions ALTER COLUMN slug SET NOT NULL;
ALTER TABLE sessions ADD CONSTRAINT uq_sessions_club_slug UNIQUE (club_id, slug);
