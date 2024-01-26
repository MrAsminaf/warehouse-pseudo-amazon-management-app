import psycopg2

# Connection to database
conn = psycopg2.connect(
    host = 'localhost',
    database = 'factory',
    user = 'postgres',
    password = 'password',
    port ='5432'
)

# Initial tables for database
teams_table_init = '''
    CREATE TABLE Teams(
        team_id serial PRIMARY KEY,
        boss_id bigserial,
        shift VARCHAR(50) NOT NULL
    )   
'''

workers_table_init = '''
    CREATE TABLE Workers(
        worker_id bigserial PRIMARY KEY,
        team_id INTEGER REFERENCES Teams (team_id),
        position VARCHAR(50) NOT NULL,
        name VARCHAR(50) NOT NULL,
        surname VARCHAR(50) NOT NULL,
        email VARCHAR(50) NOT NULL CHECK (email ~* '^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+[.][A-Za-z]+$'),
        join_date timestamp NOT NULL
    )   
'''

clients_table_init = '''
    CREATE TABLE Clients(
        client_id bigserial PRIMARY KEY,
        name VARCHAR(50) NOT NULL,
        surname VARCHAR(50) NOT NULL,
        email VARCHAR(50) UNIQUE NOT NULL CHECK (email ~* '^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+[.][A-Za-z]+$'),
        phone_number VARCHAR(20) ,
        adress VARCHAR(50) NOT NULL,
        payment_method VARCHAR(50) NOT NULL,
        join_date timestamp NOT NULL
    )   
'''

products_table_init = '''
    CREATE TABLE Products(
        product_id bigserial PRIMARY KEY,
        name VARCHAR(50) UNIQUE NOT NULL,
        category VARCHAR(50) NOT NULL,
        amount SMALLINT NOT NULL,
        price NUMERIC(7,2) NOT NULL,
        size VARCHAR(20) NOT NULL,
        location VARCHAR(20) NOT NULL,
        edit_date timestamp NOT NULL
    )   
'''

carts_table_init = '''
    CREATE TABLE Carts(
        chart_id bigserial,
        product_id bigserial REFERENCES Products (product_id),
        amount SMALLINT NOT NULL,
        client_id bigserial REFERENCES Clients (client_id),
        PRIMARY KEY(chart_id, product_id)
    )   
'''

# Creating initial empty database
def create_starter_db():
    cur = conn.cursor()

    cur.execute(teams_table_init);
    cur.execute(workers_table_init);
    cur.execute(clients_table_init);
    cur.execute(products_table_init);
    cur.execute(carts_table_init);

    conn.commit()
    cur.close()
    conn.close()

# Dropping initial empty database
def drop_starter_db():
    cur = conn.cursor()

    cur.execute('''DROP TABLE Teams CASCADE''');
    cur.execute('''DROP TABLE Workers CASCADE''');
    cur.execute('''DROP TABLE Clients CASCADE''');
    cur.execute('''DROP TABLE Products CASCADE''');
    cur.execute('''DROP TABLE Carts CASCADE''');

    conn.commit()
    cur.close()
    conn.close()

# create_starter_db()
# drop_starter_db()