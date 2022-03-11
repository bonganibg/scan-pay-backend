-- User Table
create table Users
(
    user_id varchar(40) primary key,
    full_name varchar(50),
    username varchar(30) not null,
    email varchar(40) not null,
    phone_number varchar(12),
    password varchar(50) not null,  -- will be hashed
    reset_pass_phrase varchar(100) not null -- Generated when the user creates an account, used to reset the password
)

-- Business
create table Business
(
business_id varchar(40) primary key,
name varchar(50) not null,
email varchar(40) not null,
phone_number varchar(12) not null,
password varchar(50) not null,
reset_pass_phrase varchar(100) not null,
)

-- Stripe api
create table StripeApi
(
	business_id varchar(40) primary key foreign key references Business(business_id),
	api_key varchar(150) not null,
)


-- Store
create table Store
(
  store_id varchar(40) primary key,
  business_id varchar(40) not null foreign key references Business(business_id),
  name varchar(40) not null,
  qr_code varchar(30) not null, -- Used to find the store page being bought from
  image_uri varchar(300)
)

-- products
create table Product
(
  product_id varchar(40) primary key,
  qr_code varchar(30) not null,
  name varchar(60) not null,
  price money not null,
  description varchar(250),
  image_uri varchar(100), -- link to the image which is stored somewhere else
  sale_price money,
  sale bit,
  stock int
)

-- Store Products
create table Store_Product
(
  product_id varchar(40) foreign key references Product(product_id),
  store_id varchar(40) foreign key references Store(store_id)
)


-- Receipt
create table Receipt
(
  receipt_id varchar(40) primary key,
  store_id varchar(40) foreign key references Store(store_id),
  user_id varchar(40) foreign key references Users(user_id),
  qr_code varchar(30) not null, --Used to index a customers receipt
  total_price money not null,
  billing_date date not null,
)


-- Product Receipt
create table Product_Receipt
(
  receipt_id varchar(40) foreign key references Receipt(receipt_id),
  product_id varchar(40) foreign key references Product(product_id),
  price money not null,
  quantity int not null,
  total decimal
)


-- Shopping Cart
create table Shopping_Cart
(
  cart_id varchar(40) primary key,
  user_id varchar(40) foreign key references Users(user_id),
  store_id varchar(40) foreign key references Store(store_id)
)


-- Product Cart
create table Product_Cart
(
  cart_id varchar(40) foreign key references Shopping_Cart(cart_id),
  product_id varchar(40) foreign key references Product(product_id),
  quantity int not null
)
