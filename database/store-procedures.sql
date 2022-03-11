-- Get business stores
create procedure getBusinessStores
(
 @BusinessID varchar(40)
)
as begin
 select name, qr_code, store_id, image_uri
 from Store
 where business_id = @BusinessID
end

-- Create Store
create procedure manageStoreInformation
(
  @StoreID varchar(50),
  @BusinessID varchar(50),
  @Name varchar(40),
  @QrCode varchar(30),
  @ImageUri varchar(300)
)
as begin
  insert into Store values (@StoreID, @BusinessID, @name, @QrCode, @ImageUri)
end


-- Get Store
  create procedure getStoreDetails
  (
    @StoreID varchar(40)
  )
  as begin
    select store_id, name, qr_code, image_uri from Store
    where store_id = @StoreID
  end


-- Update Store information
  create procedure updateStoreInformation
  (
    @StoreID varchar(50),
    @BusinessID varchar(50),
    @Name varchar(40),
    @QrCode varchar(30),
    @ImageUri varchar(50)
  )
  as begin
    update Store
    set name = @Name,
    qr_code = @QrCode,
    image_uri = @ImageUri
    where store_id = @StoreID
    and business_id = @BusinessID
  end


  -- Get the products in a store
    create procedure getStoreProducts
    (
      @StoreID varchar(40)
    )
    as begin
      select *
      from Product P
      left join Store_Product SP on P.product_id = SP.product_id
      where SP.store_id = @StoreID
    end
