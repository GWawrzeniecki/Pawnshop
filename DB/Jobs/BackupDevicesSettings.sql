sp_addumpdevice 'disk', 'FullBackup', 'c:\backup\fullBackup.bak';

sp_addumpdevice 'disk', 'DifferentialBackup', 'c:\backup\differentialBackup.bak';

sp_addumpdevice 'disk', 'TransactionLogBackup', 'c:\backup\transactionLogBackup.trn';

BACKUP Database Pawnshop 
TO FullBackup;

BACKUP Database Pawnshop 
TO DifferentialBackup
WITH DIFFERENTIAL;

BACKUP LOG Pawnshop 
TO TransactionLogBackup;
