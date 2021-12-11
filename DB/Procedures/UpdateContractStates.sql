ALTER PROCEDURE [Pawnshop].UpdateContractStates
AS
BEGIN
	UPDATE Pawnshop.Contract 
	SET ContractStateID = (
			SELECT ID
			FROM Pawnshop.ContractState
			WHERE State = 'Niewykupiona'
			)
	WHERE (
			(ContractStateID = (
				SELECT ID
				FROM Pawnshop.ContractState
				WHERE State = 'Założona'
				) -- contract created not renewed and StartDate plus Lending rate days is older than actual date
			AND datediff(day, DATEADD(day, (
						SELECT lr.Days
						FROM Pawnshop.LendingRate lr
						WHERE lr.ID = LendingRateID
						), StartDate), getdate()) > 0)
			OR (
				ContractStateID = (
					SELECT ID
					FROM Pawnshop.ContractState
					WHERE State = 'Przedłużona'
					) -- contract renewed and last StarDate from the latest renew plus Lending rate days is older than actual date
				AND (DATEDIFF(day, dateadd(day, (
							SELECT lr.Days
							FROM Pawnshop.LendingRate lr
							WHERE lr.ID = (
									SELECT cc.LendingRateID
									FROM Pawnshop.ContractRenew cc
									WHERE cc.ContractNumberID = Contract.ContractNumberID
										AND cc.StartDate = (
											SELECT MAX(cc.StartDate)
											FROM Pawnshop.ContractRenew cc
											WHERE cc.ContractNumberID = Contract.ContractNumberID
											)
									)
							), (
							SELECT MAX(cc.StartDate)
							FROM Pawnshop.ContractRenew cc
							WHERE cc.ContractNumberID = Contract.ContractNumberID
							)), getdate()) > 0)
				)
			)
END