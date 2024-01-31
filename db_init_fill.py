from random import choices # to random choose an element from a list
from csv import reader # to work with csv data
import datetime as dt # to use current date in time variables
import psycopg2 # to communicate with postgre sql server
from random import randint # to generate random records for database

# Classes:

# represents single row in teams table
class Team():
    def __init__(self, team_id=None, boss_id=None, shift=None):
        self.team_id = team_id
        self.boss_id = boss_id
        self.shift = shift
        self.data = [self.team_id, self.boss_id, self.shift]

    def __repr__(self):
        return f"{self.data}"

# represents teams table
class Teams():
    def __init__(self, teams=[]):
        self.teams = teams

    def amount(self):
        return len(self.teams)

    def __repr__(self):
        return f"The table represents a data of {self.amount()} teams"

    # adding new row
    def add_team(self, team_id, boss_id, shift):
        team_id = self.amount() + 1
        team = Team(team_id, boss_id, shift)
        self.teams.append(team)

# represents single row in workers table
class Worker():
    def __init__(self, worker_id=None, team_id=None, position=None, name=None, surname=None, email=None, join_date=None):
        self.worker_id = worker_id
        self.team_id = team_id
        self.position = position
        self.name = name
        self.surname = surname
        self.email = email
        self.join_date = join_date
        self.sex = None
        self.data = [self.worker_id, self.team_id, self.position, self.name, self.surname, self.email, self.join_date]

    def __repr__(self):
        return f"{self.data}"

    # for random choice of name, surname from csv file
    def draw_sex(self):
        self.sex = choices(["male", "female"], weights=[0.4, 0.6], k=1)[0]

    # for random choice of name, surname from csv file
    def draw_person(self):
        if self.sex == None:
            self.draw_sex()
        if self.sex == "male":
            name_file = "male_names.csv"
            surname_file = "male_lastname.csv"
        else:
            name_file = "female_names.csv"
            surname_file = "female_lastname.csv"
        with open(name_file, encoding="utf8") as f:
            csv_reader = reader(f)
            next(csv_reader)  # to skip headers
            data = [row for row in csv_reader]
            names = [name[0] for name in data]
            weights = [float(weight[2]) for weight in data]
            self.name = choices(names, weights=weights, k=1)[0]
        with open(surname_file, encoding="utf8") as f:
            csv_reader = reader(f)
            next(csv_reader)  # to skip headers
            data = [row for row in csv_reader]
            surnames = [surname[0] for surname in data]
            weights = [float(weight[1]) for weight in data]
            self.surname = choices(surnames, weights=weights, k=1)[0]


# represents workers table
class Workers():
    def __init__(self, workers=[]):
        self.workers = workers

    def amount(self):
        return len(self.workers)

    def __repr__(self):
        return f"The table represents a data of {self.amount()} workers"

    # adding worker with specified values
    def add_worker(self, team_id, position, name, surname, email):
        worker_id = self.amount() + 1
        join_date = dt.date.today()
        worker = Worker(worker_id, team_id, position, name, surname, email, join_date)
        self.workers.append(worker)

    # adding worker with random values
    def add_random_worker(self):
        worker_id = self.amount() + 1
        join_date = dt.date.today()
        team_id = choices([1, 2, 3, 4, 5], k=1)[0]
        position = choices(["junior", "mid", "senior"], k=1)[0]
        worker = Worker(worker_id, team_id, position, None, None, None, join_date)
        worker.draw_person()
        worker.email = "".join((worker.name.lower(), ".", worker.surname.lower(), "@companyname.com"))
        pol_char = "ęóąśłżźćńößüä"
        eng_char = "eoaslzzcnobua"
        for i in range(len(pol_char)):
            worker.email = worker.email.replace(pol_char[i], eng_char[i])
        worker.data = [worker.worker_id, worker.team_id, worker.position, worker.name, worker.surname, worker.email, worker.join_date]
        self.workers.append(worker)

    # adding specified amount of workers with random values
    def add_random_workers(self, amount):
        for i in range(amount):
            self.add_random_worker()


# classes Client(), Clients() are very similar to previous Worker(), Workers()
# in future code should be optimised with using inheritance

# represents single row in clients table
class Client():
    def __init__(self, client_id=None, name=None, surname=None, email=None, phone_number=None, adress=None, payment_method=None, join_date=None):
        self.client_id = client_id
        self.name = name
        self.surname = surname
        self.email = email
        self.phone_number = phone_number
        self.adress = adress
        self.payment_method = payment_method
        self.join_date = join_date
        self.sex = None
        self.data = [self.client_id, self.name, self.surname, self.email, self.phone_number, self.adress,
                     self.payment_method, self.join_date]

    def __repr__(self):
        return f"{self.data}"

    def draw_sex(self):
        self.sex = choices(["male", "female"], weights=[0.4, 0.6], k=1)[0]

    def draw_person(self):
        if self.sex == None:
            self.draw_sex()
        if self.sex == "male":
            name_file = "male_names.csv"
            surname_file = "male_lastname.csv"
        else:
            name_file = "female_names.csv"
            surname_file = "female_lastname.csv"
        with open(name_file, encoding="utf8") as f:
            csv_reader = reader(f)
            next(csv_reader)  # to skip headers
            data = [row for row in csv_reader]
            names = [name[0] for name in data]
            weights = [float(weight[2]) for weight in data]
            self.name = choices(names, weights=weights, k=1)[0]
        with open(surname_file, encoding="utf8") as f:
            csv_reader = reader(f)
            next(csv_reader)  # to skip headers
            data = [row for row in csv_reader]
            surnames = [surname[0] for surname in data]
            weights = [float(weight[1]) for weight in data]
            self.surname = choices(surnames, weights=weights, k=1)[0]

# represents clients table
class Clients():
    def __init__(self, clients=[]):
        self.clients = clients

    def amount(self):
        return len(self.clients)

    def __repr__(self):
        return f"The table represents a data of {self.amount()} clients"

    def add_client(self, name, surname, email, phone_number, adress, payment_method):
        client_id = self.amount() + 1
        join_date = dt.date.today()
        client = Client(client_id, name, surname, email, phone_number, adress, payment_method, join_date)
        self.clients.append(client)

    def add_random_client(self):
        client_id = self.amount() + 1
        join_date = dt.date.today()
        def cyph():
            return str(randint(0,9))
        phone_tuple = ("+48 ", cyph(), cyph(), cyph(), " ", cyph(), cyph(), cyph(), " ", cyph(), cyph(), cyph())
        phone_number = "".join(phone_tuple)
        town = choices(["Warszawa", "Kraków", "Wrocław", "Łódź", "Poznań", "Gdańsk", "Szczecin"], weights=[1861975, 803283, 675079, 658444, 541316, 486345, 391566], k=1)[0]
        street = choices(["Polna", "Leśna", "Słoneczna", "Krótka", "Szkolna", "Ogrodowa", "Lipowa", "Brzozowa", "Łąkowa", "Kwiatowa"], weights=[3090, 2999, 2414, 2270, 2175, 2172, 1684, 1552, 1508, 1480], k=1)[0]
        adress_tuple = (town, " ",street, " ", cyph(), cyph(), "/", cyph(), cyph())
        adress = "".join(adress_tuple)
        payment_method = choices(["debit card", "cash"], weights=[0.7, 0.3], k=1)[0]

        client = Client(client_id, None, None, None, phone_number, adress, payment_method, join_date)
        client.draw_person()
        client.email = "".join((client.name.lower(), ".", client.surname.lower(), "@gmail.com"))
        pol_char = "ęóąśłżźćńößüä"
        eng_char = "eoaslzzcnobua"
        for i in range(len(pol_char)):
            client.email = client.email.replace(pol_char[i], eng_char[i])
        client.data = [client.client_id, client.name, client.surname, client.email, client.phone_number, client.adress,
                     client.payment_method, client.join_date]
        self.clients.append(client)

    def add_random_clients(self, amount):
        for i in range(amount):
            self.add_random_client()


# represents single row in products table
class Product():
    def __init__(self, product_id=None, name=None, category=None, amount=None, price=None, size=None, location=None, edit_date=None):
        self.product_id = product_id
        self.name = name
        self.category = category
        self.amount = amount
        self.price = price
        self.size = size
        self.location = location
        self.edit_date = edit_date
        self.data = [self.product_id, self.name, self.category, self.amount, self.price, self.size,
                     self.location, self.edit_date]

    def __repr__(self):
        return f"{self.data}"

# represents products table
class Products():
    def __init__(self, products=[]):
        self.products = products

    def amount(self):
        return len(self.products)

    def __repr__(self):
        return f"The table represents a data of {self.amount()} products"

    # add row with specified values
    def add_product(self, name, category, amount, price, size, location):
        product_id = self.amount() + 1
        edit_date = dt.date.today()
        product = Product(product_id, name, category, amount, price, size, location, edit_date)
        self.products.append(product)

    # edit specified values in row, actualise edit_date
    def edit_product(self, product_id, name=None, category=None, amount=None, price=None, size=None, location=None):
        self.products[product_id-1].edit_date = dt.date.today()
        if name:
            self.products[product_id - 1].name = name
        if category:
            self.products[product_id - 1].category = category
        if amount:
            self.products[product_id - 1].amount = amount
        if price:
            self.products[product_id - 1].price = price
        if size:
            self.products[product_id - 1].size = size
        if location:
            self.products[product_id - 1].location = location


# represents single row in carts table
class Cart():
    def __init__(self, cart_id, product_id, amount=None, client_id=None, status="created"):
        self.cart_id = cart_id
        self.product_id = product_id
        self.amount = amount
        self.client_id = client_id
        self.status = status
        self.data = [self.cart_id, self.product_id, self.amount, self.client_id, self.status]

    def __repr__(self):
        return f"{self.data}"

# represents carts table
class Carts():
    def __init__(self, carts=[]):
        self.carts = carts
        self.carts_ids = 0

    def amount(self):
        return len(self.carts)

    def __repr__(self):
        return f"The table represents a data of {self.amount()} carts"

    # adding row with new cart_id
    def add_cart(self, product_id, amount=1, client_id=None, status="created"):
        self.carts_ids += 1
        cart_id = self.carts_ids
        cart = Cart(cart_id, product_id, amount, client_id, status)
        self.carts.append(cart)

    # adding row with old cart_id, client_id but new product_id
    def add_product(self, cart_id, product_id, amount=1, status="created"):
        a = [self.carts[i].cart_id for i in range(len(self.carts))]
        b = a.index(cart_id)
        client_id = self.carts[b].client_id
        # primary key is composite(cart_id, client_id) so if the product_id is the same it should just increase amount
        if product_id == self.carts[b].product_id:
            self.carts[b].amount += 1
        else:
            cart = Cart(cart_id, product_id, amount, client_id, status)
            self.carts.append(cart)

    # to change status to "ordered"
    def order(self, cart_id):
        for cart in self.carts:
            if cart.cart_id == cart_id:
                cart.status = "ordered"
                cart.data = [cart.cart_id, cart.product_id, cart.amount, cart.client_id, cart.status]

    # to change status to "payed"
    def pay(self, cart_id):
        for cart in self.carts:
            if cart.cart_id == cart_id:
                cart.status = "payed"
                cart.data = [cart.cart_id, cart.product_id, cart.amount, cart.client_id, cart.status]


# Filling data

# creating and filling table teams
teams = Teams()
teams.add_team(1, 2, "first")
teams.add_team(2, 3, "first")
teams.add_team(3, 4, "second")
teams.add_team(4, 5, "second")
teams.add_team(5, 6, "third")

# creating and filling table workers
workers = Workers()
workers.add_random_workers(100)

# creating and filling table workers
clients = Clients()
clients.add_random_clients(100)

# creating and filling table products
products = Products()
products.add_product("maska żelowa", "kosmetyki", 200, 62.43, "small", "Wrocław")
products.add_product("olejek do włosów", "kosmetyki", 400, 56.30, "small", "Wrocław")
products.add_product("organizer kosmetyków", "kosmetyki", 300, 75.25, "medium", "Wrocław")
products.add_product("podkładka na biurko", "artykuły biurowe", 100, 62.99, "medium", "Warszawa")
products.add_product("kartki samoprzylepne", "artykuły biurowe", 900, 45.98, "small", "Warszawa")
products.add_product("kolorowe kredki", "artykuły biurowe", 700, 77.88, "small", "Warszawa")
products.add_product("teczki na dokumenty", "artykuły biurowe", 800, 33.38, "medium", "Gdańsk")
products.add_product("długopis niebieski", "artykuły biurowe", 500, 17.71, "small", "Gdańsk")
products.add_product("taśmy do zakreślaczy", "artykuły biurowe", 600, 8.96, "small", "Gdańsk")
products.add_product("miękki kaganiec", "zwierzęta", 150, 11.11, "small", "Lublin")
products.add_product("smycz", "zwierzęta", 550, 82.01, "small", "Lublin")
products.add_product("karma dla psa", "zwierzęta", 850, 200.22, "medium", "Lublin")
products.add_product("słuchawki pchełki", "elektronika", 250, 489.49, "small", "Rzeszów")
products.add_product("AIphone 2077", "elektronika", 750, 5399.99, "small", "Rzeszów")
products.add_product("mysz komputerowa", "elektronika", 950, 280.12, "small", "Rzeszów")
products.add_product("taśma maskująca", "dom", 650, 13.94, "small", "Kraków")
products.add_product("lampa podłogowa", "dom", 450, 204.99, "big", "Kraków")
products.add_product("słuchawka prysznicowa", "dom", 350, 74.99, "medium", "Kraków")
products.add_product("fotelik samochodowy", "dzieci", 777, 1308.55, "big", "Białystok")
products.add_product("gryzak silikonowy", "dzieci", 666, 49.99, "small", "Białystok")

# creating and filling table carts
carts = Carts()
for i in range(20):
    product_id = randint(1,20)
    amount = randint(1,5)
    client_id = randint(1, 100)
    carts.add_cart(product_id, amount, client_id)
for i in range(20):
    cart_id = randint(1, 20)
    product_id = randint(1,20)
    amount = randint(1,3)
    carts.add_product(cart_id, product_id, amount)
for i in range(5):
    cart_id = randint(1, 20)
    carts.order(cart_id)
for i in range(5):
    cart_id = randint(1, 20)
    carts.pay(cart_id)

# connecting with postgre sql
conn = psycopg2.connect(
    host = 'localhost',
    database = 'factory',
    user = 'postgres',
    password = 'bekara',
    port ='5432'
)
cursor = conn.cursor()

# loops to enter records into a tables
for team in teams.teams:
    cursor.execute("INSERT into teams(team_id, boss_id, shift) VALUES (%s, %s, %s)", team.data)
for worker in workers.workers:
    cursor.execute("INSERT into workers(worker_id, team_id, position, name, surname, email, join_date) VALUES (%s, %s, %s, %s, %s, %s, %s)", worker.data)
for client in clients.clients:
    cursor.execute("INSERT into clients(client_id, name, surname, email, phone_number, adress, payment_method, join_date) VALUES (%s, %s, %s, %s, %s, %s, %s, %s)", client.data)
for product in products.products:
    cursor.execute("INSERT into products(product_id, name, category, amount, price, size, location, edit_date) VALUES (%s, %s, %s, %s, %s, %s, %s, %s)", product.data)
for cart in carts.carts:
    cursor.execute("INSERT into carts(cart_id, product_id, amount, client_id, status) VALUES (%s, %s, %s, %s, %s)", cart.data)


# Make the changes to the database persistent
conn.commit()
# Close cursor and communication with the database
cursor.close()
conn.close()
