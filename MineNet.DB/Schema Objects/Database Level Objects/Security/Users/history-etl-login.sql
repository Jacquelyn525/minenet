CREATE LOGIN [history-etl] 
  WITH PASSWORD = 'gfJft{ev4poDwev|ydD{yImjmsFT7_&#$!~<iybeQ{hJfmlg',
  CHECK_EXPIRATION=ON,
  DEFAULT_DATABASE = MineNetHistoryDB ;
GO

  CREATE USER [history-etl] FOR LOGIN [history-etl]
GO

ALTER ROLE db_owner ADD MEMBER [history-etl]
GO

-- Add some permissions the the schema for the newly created user
GRANT INSERT, SELECT, DELETE, ALTER ON SCHEMA::[minenet] TO [history-etl];
GO

-- enable the newly created user to search the current db
GRANT VIEW DEFINITION TO [history-etl]
GO
