ALTER TABLE players ADD COLUMN slug TEXT;

CREATE OR REPLACE FUNCTION generate_player_slug(p_club_id UUID, p_first_name TEXT, p_last_name TEXT)
RETURNS TEXT
LANGUAGE plpgsql
AS $$
DECLARE
  v_base TEXT;
  v_slug TEXT;
  v_suffix INT := 1;
BEGIN
  -- Combine first and last name, strip special chars, lowercase, replace spaces with hyphens
  v_base := trim(both '-' from regexp_replace(lower(trim(p_first_name) || ' ' || trim(p_last_name)), '[^a-z0-9]+', '-', 'g'));

  IF v_base = '' THEN
    v_base := 'player';
  END IF;

  v_slug := v_base;

  -- Check uniqueness within the same club only
  WHILE EXISTS (SELECT 1 FROM players WHERE club_id = p_club_id AND slug = v_slug) LOOP
    v_suffix := v_suffix + 1;
    v_slug := v_base || '-' || v_suffix;
  END LOOP;

  RETURN v_slug;
END;
$$;

-- Trigger to auto-generate slug if not provided
CREATE OR REPLACE FUNCTION players_auto_slug()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF NEW.slug IS NULL OR NEW.slug = '' THEN
    NEW.slug := generate_player_slug(NEW.club_id, COALESCE(NEW.first_name, NEW.name), COALESCE(NEW.last_name, ''));
  END IF;
  RETURN NEW;
END;
$$;

CREATE TRIGGER trigger_players_auto_slug
  BEFORE INSERT OR UPDATE ON players
  FOR EACH ROW
  EXECUTE FUNCTION players_auto_slug();

-- Generate slugs for existing players
UPDATE players 
SET slug = generate_player_slug(club_id, COALESCE(first_name, name), COALESCE(last_name, ''))
WHERE slug IS NULL;

-- Make slug required and add unique constraint per club
ALTER TABLE players ALTER COLUMN slug SET NOT NULL;
ALTER TABLE players ADD CONSTRAINT uq_players_club_slug UNIQUE (club_id, slug);
CREATE INDEX idx_players_slug ON players (slug);
CREATE INDEX idx_players_club_slug ON players (club_id, slug);
