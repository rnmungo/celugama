create table Tokens (
	ID int identity(1, 1) primary key,
	AccessToken varchar(200) unique not null,
	RefreshToken varchar(200) unique not null,
	DueDateTime datetimeoffset default getdate(),
	AppID varchar(50) not null,
	SecretKey varchar(50) not null,
	constraint token unique (AppID, SecretKey)
)
go

create table Logs (
	ID int identity(1, 1) primary key,
	StatusCode tinyint,
	Error varchar(30),
	Message text not null,
	StackTrace text,
	Resource varchar(100),
	Level varchar(10) not null,
	CreatedAt datetimeoffset default getdate()
)
go

create table Orders (
	ID int identity(1, 1) primary key,
	OrderIDML bigint unique not null,
	DateCreated datetimeoffset not null,
	DateClosed datetimeoffset,
	Nickname varchar(60) not null,
	Name varchar(60) not null,
	LastName varchar(60) not null,
	Status varchar(50) not null,
	TotalAmount decimal(10, 2) default 0.0,
	TotalAmountWithShipping decimal(10, 2) default 0.0,
	PaidAmount decimal(10, 2) default 0.0,
	ShippingID bigint,
	ShippingStatus varchar(50),
	ShippingSubstatus varchar(50),
	ShippingType varchar(50),
	ShippingAddress varchar(120),
	ShippingStreetName varchar(100),
	ShippingStreetNumber varchar(20),
	ShippingLongitude decimal(12,8),
	ShippingLatitude decimal(12,8),
	ShippingComment text,
	ShippingZipCode varchar(10),
	ShippingReceiverName varchar(100),
	ShippingReceiverPhone varchar(50),
	ShippingCity varchar(100),
	ShippingState varchar(100),
	ShippingCountry varchar(100),
	ShippingNeighborhood varchar(100),
	Observations text,
	PackID bigint
)
go

create table OrderItems (
	ID int identity(1, 1) primary key,
	OrderID int not null,
	OrderIDML bigint not null,
	ItemID varchar(60) not null,
	Title varchar(255) not null,
	CategoryID varchar(50) not null,
	VariationID varchar(50),
	VariationColor varchar(255),
	SellerSKU varchar(40),
	Quantity int not null,
	UnitPrice decimal(10, 2) not null,
	constraint FK_Items_Order foreign key (OrderID)
        references Orders (ID)
)
go

create table Payments (
	ID int identity(1, 1) primary key,
	OrderID int not null,
	OrderIDML bigint not null,
	PaymentID bigint unique not null,
	PaymentType varchar(40) not null,
	PaymentMethod varchar(40) not null,
	OperationType varchar(40) not null,
	Reason varchar(100),
	CardID bigint,
	PayerID bigint not null,
	Status varchar(50) not null,
	StatusDetail varchar(200),
	TransactionAmount decimal(10, 2) default 0.0,
	TaxesAmount decimal(10, 2) default 0.0,
	ShippingCost decimal(10, 2) default 0.0,
	OverpaidAmount decimal(10, 2) default 0.0,
	TotalPaidAmount decimal(10, 2) default 0.0,
	constraint FK_Payments_Order foreign key (OrderID)
        references Orders (ID)
)
go

create table Users (
	ID int identity(1, 1) primary key,
	UserName varchar(30) unique not null,
	Password varchar(255) not null
)
go