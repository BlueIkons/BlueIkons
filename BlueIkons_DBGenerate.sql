USE [master]
GO
/****** Object:  Database [BlueIkons]    Script Date: 12/10/2012 23:22:36 ******/
CREATE DATABASE [BlueIkons] ON  PRIMARY 
( NAME = N'BlueIkons', FILENAME = N'D:\Websites\DatabaseFiles\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\BlueIkons.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BlueIkons_log', FILENAME = N'D:\Websites\DatabaseFiles\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\BlueIkons_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BlueIkons] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BlueIkons].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BlueIkons] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [BlueIkons] SET ANSI_NULLS OFF
GO
ALTER DATABASE [BlueIkons] SET ANSI_PADDING OFF
GO
ALTER DATABASE [BlueIkons] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [BlueIkons] SET ARITHABORT OFF
GO
ALTER DATABASE [BlueIkons] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [BlueIkons] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [BlueIkons] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [BlueIkons] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [BlueIkons] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [BlueIkons] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [BlueIkons] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [BlueIkons] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [BlueIkons] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [BlueIkons] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [BlueIkons] SET  DISABLE_BROKER
GO
ALTER DATABASE [BlueIkons] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [BlueIkons] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [BlueIkons] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [BlueIkons] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [BlueIkons] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [BlueIkons] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [BlueIkons] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [BlueIkons] SET  READ_WRITE
GO
ALTER DATABASE [BlueIkons] SET RECOVERY SIMPLE
GO
ALTER DATABASE [BlueIkons] SET  MULTI_USER
GO
ALTER DATABASE [BlueIkons] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [BlueIkons] SET DB_CHAINING OFF
GO
USE [BlueIkons]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 12/10/2012 23:22:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Tx_Key] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [money] NULL,
	[Gift_Key] [int] NULL,
	[Init_date] [datetime] NULL,
	[Collected_date] [datetime] NULL,
	[txn_id] [nvarchar](50) NULL,
	[Tx_Status] [int] NULL,
	[pakey] [nvarchar](50) NULL,
	[receiver_email] [nvarchar](100) NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Tx_Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gift]    Script Date: 12/10/2012 23:22:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gift](
	[Gift_Key] [int] IDENTITY(1,1) NOT NULL,
	[sender_fbid] [bigint] NULL,
	[receiver_fbid] [bigint] NULL,
	[receiver_email] [nvarchar](100) NULL,
	[witty_message] [nvarchar](300) NULL,
	[created_date] [datetime] NULL,
	[blueikon] [int] NULL,
	[fbpost] [bit] NULL,
	[receiver_name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Gift] PRIMARY KEY CLUSTERED 
(
	[Gift_Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FBUsers]    Script Date: 12/10/2012 23:22:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FBUsers](
	[fbid] [bigint] NOT NULL,
	[first_name] [nvarchar](100) NULL,
	[last_name] [nvarchar](100) NULL,
	[fbemail] [nvarchar](100) NULL,
	[paypalemail] [nvarchar](100) NULL,
	[created_date] [datetime] NULL,
	[last_changed] [datetime] NULL,
	[Access_Token] [nvarchar](200) NULL,
 CONSTRAINT [PK_FBUsers] PRIMARY KEY CLUSTERED 
(
	[fbid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Charity]    Script Date: 12/10/2012 23:22:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Charity](
	[Charity_Name] [nvarchar](100) NULL,
	[Charity_Description] [nvarchar](500) NULL,
	[Charity_Email] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[View_Transactions_GiftKey]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[View_Transactions_GiftKey]
	@giftkey int
AS
BEGIN
	SELECT *
	FROM Transactions
	WHERE Gift_Key = @giftkey
	
END
GO
/****** Object:  StoredProcedure [dbo].[View_Transactions]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[View_Transactions]
	@txkey int
AS
BEGIN
	SELECT *
	FROM Transactions
	WHERE Tx_Key = @txkey
	
END
GO
/****** Object:  StoredProcedure [dbo].[View_Gift]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[View_Gift]
	@Gift_Key int
AS
BEGIN
	SELECT *
	FROM Gift
	WHERE Gift_Key = @Gift_Key
	
END
GO
/****** Object:  StoredProcedure [dbo].[View_FBUser]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[View_FBUser]
	@fbid bigint
AS
BEGIN
	SELECT *
	FROM FBUsers
	WHERE fbid = @fbid
	
END
GO
/****** Object:  StoredProcedure [dbo].[View_fbidpendinggifts]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[View_fbidpendinggifts]
	@fbid bigint
AS
BEGIN
	SELECT Gift.Gift_Key
	FROM Gift
	INNER JOIN Transactions
	ON Gift.Gift_Key = Transactions.Gift_Key
	WHERE receiver_fbid = @fbid
	AND Tx_Status = 2
	
END
GO
/****** Object:  StoredProcedure [dbo].[View_Charity]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[View_Charity]
	@Gift_Key int
AS
BEGIN
	SELECT *
	FROM Charity	
	
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Transaction_pakey]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Update_Transaction_pakey]
	@Tx_Key int = NULL,		
	@pakey nvarchar(50)

AS
BEGIN

			UPDATE Transactions
			SET
			pakey = @pakey
			WHERE Tx_Key = @Tx_Key
						

END
GO
/****** Object:  StoredProcedure [dbo].[Update_Transaction_expired]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Update_Transaction_expired]
	@Tx_Key int = NULL

AS
BEGIN

			UPDATE Transactions
			SET
			Tx_Status = 4
			WHERE Tx_Key = @Tx_Key
						

END
GO
/****** Object:  StoredProcedure [dbo].[Update_Transaction]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_Transaction]
	@Tx_Key int = NULL,	
	@Amount money,
	@Tx_Key_Return int Output,	
	@Gift_Key int,
	@Tx_Status int,
	@txn_id nchar(19),
	@receiver_email nvarchar(100)

AS
BEGIN


SET @Tx_Key_Return = 0

IF @Tx_Status = 1 --Clicked on order form & went to paypal / Initiate tx
	BEGIN
	
	DECLARE @CurrentDate datetime
	SET @CurrentDate = getdate()

	INSERT INTO Transactions
	(Gift_Key,Amount,Tx_Status,Init_date)
	Values
	(@Gift_Key,@Amount,1,@CurrentDate)
	
	SET @Tx_Key_Return = (SELECT Tx_Key FROM Transactions WHERE Init_Date = @CurrentDate)
	END

IF @Tx_Status = 2 --Pre approval completed
	BEGIN
		UPDATE Transactions
			SET
			Tx_Status = 2
			WHERE Tx_Key = @Tx_Key
	END
IF @Tx_Status = 3 --Payment Sent
	BEGIN
	--DECLARE @Check_Tx_Key int
	--SET @Check_Tx_Key = (SELECT Tx_Key FROM Transactions WHERE Tx_Key = @Tx_Key)
	--	IF @Check_Tx_Key <> NULL
	--		BEGIN
			UPDATE Transactions
			SET
			Tx_Status = 3,		
			Collected_date = getdate(),			
			txn_id = @txn_id,
			receiver_email = @receiver_email
			WHERE Tx_Key = @Tx_Key
						
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Update_Gift_Receiver]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Update_Gift_Receiver]
	@Gift_Key int,
	@receiverfbid bigint

AS
BEGIN

			UPDATE Gift
			SET				
				receiver_fbid = @receiverfbid
			WHERE Gift_Key = @Gift_Key
			

END
GO
/****** Object:  StoredProcedure [dbo].[Update_Gift]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_Gift]
	@Gift_Key int,
	@senderfbid bigint,
	@receiverfbid bigint,
	@receiveremail nvarchar(100),
	@witty nvarchar(300),
	@blueikon int,
	@Gift_Key_Return int Output,
	@fbpost bit,
	@receivername nvarchar(100)

AS
BEGIN

If @Gift_Key = 0 --Create new record
	BEGIN
	
	DECLARE @CurrentDate datetime
	SET @CurrentDate = getdate()

	INSERT INTO Gift
	(sender_fbid, receiver_fbid, receiver_email, witty_message, created_date, blueikon,fbpost,receiver_name)
	VALUES
	(@senderfbid,@receiverfbid, @receiveremail, @witty, @CurrentDate, @blueikon,@fbpost,@receivername)

	SET @Gift_Key_Return = (SELECT Gift_Key FROM Gift WHERE created_Date = @CurrentDate)
	END
ELSE
	BEGIN	
			UPDATE Gift
			SET
				sender_fbid = @senderfbid,
				receiver_fbid = @receiverfbid,
				receiver_email = @receiveremail,
				witty_message = @witty,
				blueikon = @blueikon,
				fbpost = @fbpost,
				receiver_name = @receivername
			WHERE Gift_Key = @Gift_Key
			
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Update_FBUser]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_FBUser]
	@FBid bigint,
	@First_Name varchar(50),
	@Last_Name varchar(50),
	@Email varchar(100),	
	@Access_Token nvarchar(200)

AS
BEGIN
DECLARE @Check_FBid bigint
SET @Check_FBid = (SELECT FBid FROM FBUsers WHERE FBid = @FBid)

If @Check_FBid is null --Create new record
	BEGIN
	INSERT INTO FBUsers
	(FBid,First_Name,Last_Name,fbemail,created_date,Last_Changed,Access_Token)
	VALUES
	(@FBid,@First_Name,@Last_Name,@Email,getdate(),getdate(),@Access_Token)
	END
ELSE
	BEGIN
	If @Access_Token = '' --No Access Token was read
		BEGIN
			UPDATE FBUsers
			SET
				First_Name = @First_Name,
				Last_Name = @Last_Name,
				fbemail = @Email,
				Last_Changed = getdate()							
			WHERE FBid=@FBid
		END
	ELSE
		BEGIN
			UPDATE FBUsers
			SET
				First_Name = @First_Name,
				Last_Name = @Last_Name,
				fbemail = @Email,
				Last_Changed = getdate(),
				Access_Token = @Access_Token
			WHERE FBid=@FBid
		END	
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Update_Charity]    Script Date: 12/10/2012 23:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Update_Charity]	
	@Charity_Name nvarchar(100),
	@Charity_Description nvarchar(500),
	@Charity_Email nvarchar(100)

AS
BEGIN
	
		UPDATE Charity
			SET
			Charity_Name = @Charity_Name,
			Charity_Description = @Charity_Description,
			Charity_Email = @Charity_Email			

END
GO
