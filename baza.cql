CREATE KEYSPACE IF NOT EXISTS fitness_app
WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};

USE fitness_app;

CREATE TABLE IF NOT EXISTS user_by_email (
    email         text,
    user_id       uuid,
    password_hash text,
    first_name    text,
    last_name     text,
    gender        text,
    birth_date    date,
    PRIMARY KEY (email)
);

CREATE TABLE IF NOT EXISTS user_by_id (
    user_id       uuid,
    email         text,
    password_hash text,
    first_name    text,
    last_name     text,
    gender        text,
    birth_date    date,
    PRIMARY KEY (user_id)
);

CREATE TABLE IF NOT EXISTS workouts_by_user (
    user_id      uuid,
    workout_date timestamp,
    workout_id   timeuuid,
    workout_type text,
    is_rest_day  boolean,
    notes        text,
    PRIMARY KEY (user_id, workout_date, workout_id)
) WITH CLUSTERING ORDER BY (workout_date DESC, workout_id DESC);

CREATE TABLE IF NOT EXISTS exercises_by_workout (
    workout_id   timeuuid,
    exc_order    int,
    name         text,
    reps         int,
    rest_minutes int,
    sets         int,
    weight_kg    decimal,
    PRIMARY KEY (workout_id, exc_order)
);

CREATE TABLE IF NOT EXISTS body_metrics_by_user (
    user_id     uuid,
    body_part   text,
    metric_id   timeuuid,
    recorded_at timestamp,
    value       decimal,
    PRIMARY KEY ((user_id, body_part), metric_id)
) WITH CLUSTERING ORDER BY (metric_id DESC);

CREATE TABLE IF NOT EXISTS diet_by_user (
    user_id     uuid,
    log_day     date,
    breakfast   text,
    lunch       text,
    dinner      text,
    snacks      list<text>,
    supplements list<text>,
    calories    int,
    PRIMARY KEY (user_id, log_day)
) WITH CLUSTERING ORDER BY (log_day DESC);

CREATE TABLE IF NOT EXISTS user_health (
    user_id      uuid,
    chronic_cond list<text>,
    height_cm    decimal,
    notes        text,
    weight_kg    decimal,
    PRIMARY KEY (user_id)
);

CREATE TABLE IF NOT EXISTS period_by_user (
    user_id    uuid,
    start_date date,
    end_date   date,
    notes      text,
    PRIMARY KEY (user_id, start_date)
) WITH CLUSTERING ORDER BY (start_date DESC);

CREATE TABLE IF NOT EXISTS refresh_tokens (
    token_value text,
    user_id     uuid,
    expires_at  timestamp,
    PRIMARY KEY (token_value)
);