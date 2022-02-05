CREATE PROCEDURE cast_SAS_timestamp_to_datetime2
AS
      /*
            SAS appears to not round to the next minut until a full 61 seconds has occurred and results in a casting error within SQLServer.
            This stored procedure will identify situations where the seconds portion is equal to 60, advance the minute by 1, and then set the seconds to 00.
            The "corrected" timestamp will be cast as a SQL DATETIME2 and inserted into the LF_EFT_OCC_DTS field.
      */
      UPDATE LN83
            SET LF_EFT_OCC_DTS = CASE WHEN SUBSTRING(varchar_LF_EFT_OCC_DTS,18,2) > 1 THEN DATEADD(MINUTE, 1, CAST(REPLACE(varchar_LF_EFT_OCC_DTS, ':60.', ':00.') AS DATETIME2)) ELSE CAST(varchar_LF_EFT_OCC_DTS AS DATETIME2) END  
      FROM
            CDW..LN83_EFT_TO_LON LN83
      WHERE
            LN83.LF_EFT_OCC_DTS is NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[cast_SAS_timestamp_to_datetime2] TO [db_executor]
    AS [dbo];

