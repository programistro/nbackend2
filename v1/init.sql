CREATE TABLE users (
    id UUID PRIMARY KEY,
    balance BIGINT,
    geo TEXT,
    invited_id TEXT,
    premium BOOLEAN,
    telegram_id INTEGER,
    telegram_name TEXT,
    referals TEXT,
    tonuser_id TEXT,
    wallet_id TEXT
);

CREATE TABLE miners (
    id UUID PRIMARY KEY,
    earn TEXT,
    timestamp TIMESTAMPTZ
);

CREATE TABLE tasks (
    id UUID PRIMARY KEY,
    description TEXT,
    icon BYTEA,
    price DECIMAL,
    title TEXT
);