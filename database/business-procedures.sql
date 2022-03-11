-- Create accounts
create procedure manageBusinessAccount
(
  @BusinessID varchar(40), -- generated in the API when creating account
  @Name varchar(50),
  @Email varchar(40),
  @PhoneNumber varchar(12),
  @Password varchar(50), -- hashed password
  @PassPhrase varchar(100)
)
as begin
  insert into Business values (@BusinessID, @Name, @Email,@PhoneNumber, @Password, @PassPhrase);
end

-- Login
create procedure businessLogin
(
  @Email varchar(40),
  @Password varchar(50)
)
as begin
  select business_id from Business
  where email = @Email
  and password = @Password

  -- Get business information
  create procedure getBusinessInfo
  (
    @BusinessID varchar(40)
  )
  as begin
    select name, email, phone_number
    from Business
    where business_id = @BusinessID
  end

  -- Update business information
  create procedure updateBusinessInfo
  (
    @BusinessID varchar(40), -- generated in the API when creating account
    @Name varchar(50),
    @Email varchar(40),
    @PhoneNumber varchar(12),
    @Password varchar(50) -- hashed password
  )
  as begin
    update Business
    set name = @Name,
    email = @Email,
    phone_number = @PhoneNumber,
    password = @Password
  end


  -- Write stripe api key
   create procedure writeStripeKey
   (
     @BusinessID varchar(40),
     @StripeApi varchar(150)
   )
   as begin
     insert into StripeApi values (@BusinessID, @StripeApi)
   end

   -- Get the api key for the stripe account
   create procedure getStripeApi
   (
     @BusinessID varchar(40)
   )
   as begin
     select api_key
     from StripeApi
     where business_id = @BusinessID
   end
