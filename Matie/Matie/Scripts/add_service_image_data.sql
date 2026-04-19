ALTER TABLE services
    ADD COLUMN IF NOT EXISTS image_data bytea NULL;
