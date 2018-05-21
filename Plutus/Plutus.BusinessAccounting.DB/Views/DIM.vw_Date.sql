CREATE VIEW [DIM].[vw_Date]
AS
WITH cte AS 
(
SELECT 
	[d].[LongDate]
	, CONVERT(NVARCHAR(4), DATEPART(YEAR, [d].[LongDate])) YearNum
	, DATEPART(MONTH, [d].[LongDate]) MonthNum
	, DATEPART(DAY, [d].[LongDate]) DayNum
FROM [DIM].[Date] d
)
SELECT 
       cte.LongDate,
	   [cte].[YearNum] [CalendarYear],
       CASE
           WHEN cte.MonthNum IN ( 1, 2, 3 ) THEN 'Q1'
           WHEN cte.MonthNum IN ( 4, 5, 6 ) THEN 'Q2'
           WHEN cte.MonthNum IN ( 7, 8, 9 ) THEN 'Q3'
           ELSE 'Q4'
       END [CalendarQuarter],
       DATENAME(MONTH, [cte].[LongDate]) [CalendarMonth],
       cte.MonthNum [CalendarMonthNum],
       CASE
           WHEN cte.MonthNum IN ( 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ) THEN 'CY ' + CONVERT(NVARCHAR(4), cte.YearNum - 1) + '-' + cte.YearNum
           WHEN cte.MonthNum IN ( 11, 12 ) THEN 'CY ' + cte.YearNum + '-' + CONVERT(NVARCHAR(4), cte.YearNum + 1)
       END [CompanyYear],
       CASE
           WHEN cte.MonthNum IN ( 11, 12, 1 ) THEN 'Company Q1'
           WHEN cte.MonthNum IN ( 2, 3, 4 ) THEN 'Company Q2'
           WHEN cte.MonthNum IN ( 5, 6, 7 ) THEN 'Company Q3'
           ELSE 'Company Q4'
       END AS [CompanyQuarter],
       DATENAME(MONTH, [cte].[LongDate]) [CompanyMonth],
       CASE cte.MonthNum
           WHEN 11 THEN -2
           WHEN 12 THEN -1
           ELSE cte.MonthNum
       END + 2 [CompanyMonthNum],
       CASE
           WHEN [cte].[LongDate] <= '2016-10-31' THEN 0.2
           WHEN [cte].[LongDate] > '2016-10-31' AND [cte].[LongDate] <= '2017-10-31' THEN 0.194137
           ELSE 0.19
       END AS CTPct,
       CASE
           WHEN cte.MonthNum IN ( 1, 2, 3 ) THEN 'FY ' + CONVERT(NVARCHAR(4),cte.YearNum - 1) + '-' + cte.YearNum
           WHEN cte.MonthNum IN ( 4 ) AND cte.DayNum <= 5 THEN 'FY ' + CONVERT(NVARCHAR(4), cte.YearNum - 1) + '-' + cte.YearNum
           WHEN cte.MonthNum IN ( 4 ) AND cte.DayNum > 5 THEN 'FY ' + cte.YearNum + '-' + CONVERT(NVARCHAR(4), cte.YearNum + 1)
           WHEN cte.MonthNum IN ( 5, 6, 7, 8, 9, 10, 11, 12 ) THEN 'FY ' + cte.YearNum + '-' + CONVERT(NVARCHAR(4), cte.YearNum + 1)
       END [FinYear],
       CASE
           WHEN cte.MonthNum IN ( 4 ) AND cte.DayNum <= 5 THEN 'FY Q4'
           WHEN cte.MonthNum IN ( 4 ) AND cte.DayNum > 5 THEN 'FY Q1'
           WHEN cte.MonthNum IN ( 5, 6 ) THEN 'FY Q1'
           WHEN cte.MonthNum IN ( 7, 8, 9 ) THEN 'FY Q2'
           WHEN cte.MonthNum IN ( 10, 11, 12 ) THEN 'FY Q3'
           ELSE 'FY Q4'
       END AS [FinQuarter],
       DATENAME(MONTH, [cte].[LongDate]) [FinMonth],
       CASE cte.MonthNum
           WHEN 1 THEN 10
           WHEN 2 THEN 11
           WHEN 3 THEN 12
           ELSE cte.MonthNum
       END + 2 [FinMonthNum]
FROM cte