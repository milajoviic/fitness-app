CREATE TABLE user_by_email 
(
	email text, 
	user_id uuid, 
	password_hash text, 
	first_name text, 
	last_name text, 
	gender text, 
	birth_date date, 
	last_period_date date, 
	PRIMARY KEY (email)
);

CREATE TABLE IF NOT EXISTS workouts_by_user 
(
	user_id uuid, 
	workout_date timestamp, 
	workout_id timeuuid, 
	workout_type text, 
	is_rest_day boolean, 
	notes text, 
	PRIMARY KEY (user_id, workout_date, workout_id)
) WITH CLUSTERING ORDER BY (workout_date DESC, workout_id DESC);

CREATE TABLE IF NOT EXISTS body_metrics_by_user (
    user_id     uuid,
    body_part_ text,
    recorded_at timestamp,
    value       decimal,
    PRIMARY KEY ((user_id, body_part_), recorded_at)
) WITH CLUSTERING ORDER BY (recorded_at DESC);

CREATE TABLE IF NOT EXISTS diet_by_user 
(
	user_id uuid, 
	log_day date, 
	breakfast text, 
	lunch text, 
	dinner text, 
	snacks list<text>, 
	supplements list<text>,
	calories int, 
	PRIMARY KEY ((user_id, log_day))
);