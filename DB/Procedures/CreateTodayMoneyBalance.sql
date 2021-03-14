USE [Pawnshop]
GO
/****** Object:  StoredProcedure [Pawnshop].[CreateTodayMoneyBalance]    Script Date: 2/26/2021 7:08:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Pawnshop].[CreateTodayMoneyBalance]
AS


BEGIN
	DECLARE @moneyBalance DECIMAL(10, 2);

	IF NOT EXISTS (
			SELECT 1
			FROM Pawnshop.MoneyBalance m
			WHERE m.TodayDate = CAST( GETDATE() AS Date )
			)
	BEGIN
		IF EXISTS (
				SELECT 1
				FROM Pawnshop.MoneyBalance
				)
		BEGIN
			SELECT @moneyBalance = MoneyBalance
			FROM Pawnshop.MoneyBalance m
			WHERE m.TodayDate = (
					SELECT max(TodayDate)
					FROM Pawnshop.MoneyBalance
					)

			INSERT INTO Pawnshop.MoneyBalance
			VALUES (
				GETDATE()
				,@moneyBalance
				);
		END
		ELSE
		BEGIN
			INSERT INTO Pawnshop.MoneyBalance
			VALUES (
				Getdate()
				,0
				);
		END
	END
END;

