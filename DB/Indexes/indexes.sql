CREATE UNIQUE INDEX IX_WorkerBossLoginWorkerBossTypeIDPrivilegeIDHash ON Boss.WorkerBoss (LOGIN) include (
	WorkerBossTypeID
	,PrivilegeID
	,HASH
	);

CREATE UNIQUE INDEX IX_ContractStateState ON Worker.ContractState (STATE);

CREATE INDEX IX_ContractRenewContractNumberIDStartDateLendingRateID ON Worker.ContractRenew (
	ContractNumberID
	,StartDate
	) include (LendingRateID);

CREATE INDEX IX_SaleIsSoldSaleDateSoldPrice ON Worker.Sale (
	IsSold
	,SaleDate
	) include (SoldPrice);

CREATE INDEX IX_DealDocumentCostRepaymentCapitalProfitMoneyBalanceIdIncome ON Worker.DealDocument (
	Cost
	,RepaymentCapital
	,Profit
	,MoneyBalanceID
	,Income
	) include (PaymentID);

CREATE INDEX IX_ContractRenewContractNumberIDLendingRateIDStartDateClientID ON Worker.ContractRenew (ContractNumberID) include (
	LendingRateID
	,StartDate
	,ClientID
	);

CREATE INDEX IX_ContractItemCategoryIDContractItemIDAmountContractItemStateIDContractNumberIDDescriptionEstimatedValueNameTechnicalCondition ON Worker.ContractItem (CategoryID) include (
	Amount
	,ContractItemStateID
	,ContractNumberID
	,Description
	,EstimatedValue
	,Name
	,TechnicalCondition
	);

CREATE INDEX IX_ContractRenewLendingRateIDClientIDContractNumberIDStartDate ON Worker.ContractRenew (LendingRateID) include (
	ClientID
	,ContractNumberID
	,StartDate
	);

CREATE INDEX IX_ContractItemContractNumberIDAmountContractItemStateIDDescEstimatedValueCondCatId ON Worker.ContractItem (ContractNumberID) include (
	Amount
	,ContractItemStateID
	,Description
	,EstimatedValue
	,Name
	,TechnicalCondition
	,CategoryID
	);

CREATE INDEX IX_PersonIDFirstNameLastNameAddressIDBirthDate ON Worker.Person (
	PersonID
	,FirstName
	,LastName
	) include (
	AddressID
	,BirthDate
	);

CREATE INDEX IX_PersonLastNameFullTable ON Worker.Person (LastName) include (
	PersonID
	,AddressID
	,FirstName
	,BirthDate
	);

CREATE INDEX IX_PersonFirstNameFullTable ON Worker.Person (FirstName) include (
	PersonID
	,AddressID
	,LastName
	,BirthDate
	);

CREATE INDEX IX_ClientPeselFullTable ON Worker.Client (Pesel) include (
	ClientID
	,IDCardNumber
	,ValidityDateIDCard
	);

CREATE INDEX IX_ContractStartDateContractNumberID ON worker.Contract (StartDate) include (ContractNumberID);

CREATE NONCLUSTERED INDEX IX_ContractStateIDFullTable ON [Worker].[Contract] ([ContractStateID]) INCLUDE (
	[CreateContractDealDocumentID]
	,[BuyBackDealDocumentID]
	,[LendingRateID]
	,[DealMakerID]
	,[BuyBackID]
	,[WorkerBossID]
	,[StartDate]
	,[AmountContract]
	)

CREATE NONCLUSTERED INDEX IX_ContractContractAmountFullTable ON [Worker].[Contract] (AmountContract) INCLUDE (
	[CreateContractDealDocumentID]
	,[BuyBackDealDocumentID]
	,[LendingRateID]
	,[DealMakerID]
	,[BuyBackID]
	,[WorkerBossID]
	,[StartDate]
	,[ContractStateID]
	)

CREATE NONCLUSTERED INDEX IX_ContractLendingRateIDFullTable ON [Worker].[Contract] (LendingRateID) INCLUDE (
	[CreateContractDealDocumentID]
	,[BuyBackDealDocumentID]
	,AmountContract
	,[DealMakerID]
	,[BuyBackID]
	,[WorkerBossID]
	,[StartDate]
	,[ContractStateID]
	)

CREATE NONCLUSTERED INDEX IX_ContractStartDateFullTable ON [Worker].[Contract] (StartDate) INCLUDE (
	[CreateContractDealDocumentID]
	,[BuyBackDealDocumentID]
	,AmountContract
	,[DealMakerID]
	,[BuyBackID]
	,[WorkerBossID]
	,LendingRateID
	,[ContractStateID]
	)

CREATE INDEX IX_ContractContractStateIDContractNumberID ON Worker.Contract (ContractStateID) include (ContractNumberID);

CREATE INDEX IX_SaleContractItemIDSalePricePutOnSaleDateQuantitySoldPriceSaleDateIsSold ON Worker.Sale (ContractItemID) include (
	SalePrice
	,PutOnSaleDate
	,Quantity
	,SoldPrice
	,SaleDate
	,IsSold
	);

CREATE INDEX IX_LaptopContractItemIDFullTable ON Worker.Laptop (ContractItemID) include (
	Brand
	,Procesor
	,RAM
	,MassStorage
	,DriveType
	,DescriptionKit
	);

CREATE INDEX IX_TelephoneContractItemIDFullTable ON Worker.Telephone (ContractItemID) include (
	Brand
	,Procesor
	,RAM
	,MassStorage
	,ScreenSize
	,DescriptionKit
	);

CREATE INDEX IX_GoldProductContractItemIDFullTable ON Worker.GoldProduct (ContractItemID) include (
	GoldTestID
	,TypeID
	,Grammage
	,Carat
	);

CREATE INDEX IX_SaleSaleIDLink ON Worker.Link (SaleID) include (
	Link
	,LinkID
	);

CREATE INDEX IX_LocalSaleSaleIDFullTable ON Worker.LocalSale (SaleID) include (
	Rack
	,Shelf
	);

CREATE NONCLUSTERED INDEX IX_ContractItemNameFullTable ON [Worker].[ContractItem] ([Name] ASC) INCLUDE (
	[ContractNumberID]
	,[CategoryID]
	,[ContractItemStateID]
	,[Amount]
	,[Description]
	,[TechnicalCondition]
	,[EstimatedValue]
	)
	WITH (
			PAD_INDEX = OFF
			,STATISTICS_NORECOMPUTE = OFF
			,SORT_IN_TEMPDB = OFF
			,DROP_EXISTING = OFF
			,ONLINE = OFF
			,ALLOW_ROW_LOCKS = ON
			,ALLOW_PAGE_LOCKS = ON
			,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
			) ON [PRIMARY]
GO

CREATE INDEX IX_ContractItemContractNumberIDCategoryIDFullTable ON Worker.ContractItem (
	ContractNumberID
	,CategoryID
	) include (
	Amount
	,ContractItemStateID
	,Description
	,EstimatedValue
	,Name
	,TechnicalCondition
	);

CREATE INDEX IX_ContractItemContractNumberIDEstimatedValueFullTable ON Worker.ContractItem (
	ContractNumberID
	,EstimatedValue
	) include (
	Amount
	,ContractItemStateID
	,Description
	,CategoryID
	,Name
	,TechnicalCondition
	);

CREATE NONCLUSTERED INDEX IX_ContractStartDateContractStateIDFullTable ON [Worker].[Contract] (
	StartDate
	,ContractStateID
	) INCLUDE (
	[CreateContractDealDocumentID]
	,[BuyBackDealDocumentID]
	,AmountContract
	,[DealMakerID]
	,[BuyBackID]
	,[WorkerBossID]
	,LendingRateID
	)

CREATE INDEX IX_ContractItemEstimatedCategoryIDValueFullTable ON Worker.ContractItem (
	CategoryID
	,EstimatedValue
	) include (
	Amount
	,ContractItemStateID
	,Description
	,Name
	,TechnicalCondition
	,ContractNumberID
	);

CREATE INDEX IX_ContractItemContractNumberIDEstimatedCategoryIDValueFullTable ON Worker.ContractItem (
	ContractNumberID
	,CategoryID
	,EstimatedValue
	) include (
	Amount
	,ContractItemStateID
	,Description
	,Name
	,TechnicalCondition
	);

CREATE NONCLUSTERED INDEX IX_ContractDealMakerIDFullTable ON [Worker].[Contract] ([DealMakerID]) INCLUDE (
	[CreateContractDealDocumentID]
	,[BuyBackDealDocumentID]
	,[LendingRateID]
	,[ContractStateID]
	,[BuyBackID]
	,[WorkerBossID]
	,[StartDate]
	,[AmountContract]
	)

CREATE NONCLUSTERED INDEX IX_SaleSalePriceFullTable ON [Worker].[Sale] ([SalePrice]) INCLUDE (
	[ContractItemID]
	,[PutOnSaleDate]
	,[Quantity]
	,[SoldPrice]
	,[SaleDate]
	,[IsSold]
	)

CREATE NONCLUSTERED INDEX IX_SalePutOnSaleDateFullTable ON [Worker].[Sale] ([PutOnSaleDate]) INCLUDE (
	[ContractItemID]
	,[SalePrice]
	,[Quantity]
	,[SoldPrice]
	,[SaleDate]
	,[IsSold]
	)

CREATE INDEX IX_ClientIDCardNumberFullTable ON Worker.Client (IDCardNumber) include (
	ClientID
	,Pesel
	,ValidityDateIDCard
	);

CREATE NONCLUSTERED INDEX IX_AddressStreetFullTable ON [Worker].[Address] ([Street]) INCLUDE (
	[HouseNumber]
	,[ApartmentNumber]
	,[PostCode]
	,[CountryID]
	,[CityID]
	)


