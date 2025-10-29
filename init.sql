CREATE TABLE IF NOT EXISTS product (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100),
    price NUMERIC(10,2),
    stock INT,
    created_at TIMESTAMP DEFAULT NOW()
);