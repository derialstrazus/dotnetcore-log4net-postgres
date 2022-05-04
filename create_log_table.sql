-- Table: dbo.logs

-- DROP TABLE IF EXISTS dbo.logs;

CREATE TABLE IF NOT EXISTS dbo.logs
(
    log text COLLATE pg_catalog."default",
    id integer NOT NULL DEFAULT nextval('dbo.logs_id_seq'::regclass),
    CONSTRAINT logs_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo.logs
    OWNER to postgres;