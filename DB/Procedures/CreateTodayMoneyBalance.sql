USE [Pawnshop]
GO
/****** Object:  StoredProcedure [Pawnshop].[CreateTodayMoneyBalance]    Script Date: 2/26/2021 7:08:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [PawnshopApp].[CreateTodayMoneyBalance]
AS


BEGIN
	DECLARE @moneyBalance DECIMAL(10, 2);

	IF NOT EXISTS (
			SELECT 1
			FROM Worker.MoneyBalance m
			WHERE m.TodayDate = CAST( GETDATE() AS Date )
			)
	BEGIN
		IF EXISTS (
				SELECT 1
				FROM Worker.MoneyBalance
				)
		BEGIN
			SELECT @moneyBalance = MoneyBalance
			FROM Worker.MoneyBalance m
			WHERE m.TodayDate = (
					SELECT max(TodayDate)
					FROM Worker.MoneyBalance
					)

			INSERT INTO Worker.MoneyBalance
			VALUES (
				GETDATE()
				,@moneyBalance
				);
		END
		ELSE
		BEGIN
			INSERT INTO Worker.MoneyBalance
			VALUES (
				Getdate()
				,0
				);
		END
	END
END;

