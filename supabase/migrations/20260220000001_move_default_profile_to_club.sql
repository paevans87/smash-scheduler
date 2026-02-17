-- Move the "default matchmaking profile" concept from profile-level (is_default)
-- to club-level (default_matchmaking_profile_id)

-- Step 1: Add the new column to clubs
ALTER TABLE clubs ADD COLUMN default_matchmaking_profile_id UUID;

-- Step 2: Migrate existing data from is_default flags
UPDATE clubs
SET default_matchmaking_profile_id = mmp.id
FROM match_making_profiles mmp
WHERE mmp.club_id = clubs.id
  AND mmp.is_default = TRUE;

-- Step 3: Add foreign key constraint (ON DELETE SET NULL so deleting a profile is safe)
ALTER TABLE clubs ADD CONSTRAINT fk_clubs_default_matchmaking_profile
  FOREIGN KEY (default_matchmaking_profile_id)
  REFERENCES match_making_profiles(id)
  ON DELETE SET NULL;

-- Step 4: Drop the unique index that enforced one default per club
DROP INDEX IF EXISTS idx_match_making_profiles_default_per_club;

-- Step 5: Drop the is_default column from match_making_profiles
ALTER TABLE match_making_profiles DROP COLUMN is_default;
