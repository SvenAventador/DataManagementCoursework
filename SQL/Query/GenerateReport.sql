DECLARE @TotalSoldGoods INT
SELECT @TotalSoldGoods = COUNT(DISTINCT og.GoodId)
FROM OrderGood og

PRINT '����� ������� �������: ' + CAST(@TotalSoldGoods AS VARCHAR(10))

DECLARE @GoodsSold NVARCHAR(MAX) = ''

SELECT @GoodsSold = @GoodsSold + '�����:' + CONCAT(b.BrandName, ' ' , g.NameGood) + ' - �������: ' + CAST(COUNT(og.GoodId) AS VARCHAR(10)) + CHAR(13)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id
JOIN Brand b ON g.BrandId = b.Id
GROUP BY b.BrandName, g.NameGood

PRINT @GoodsSold
DECLARE @TotalSoldBrands INT
SELECT @TotalSoldBrands = COUNT(DISTINCT g.BrandId)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id

PRINT '����� ������� �������: ' + CAST(@TotalSoldBrands AS VARCHAR(10))

DECLARE @BrandsSold NVARCHAR(MAX) = ''

SELECT @BrandsSold = @BrandsSold + '�����: ' + b.BrandName + ' - �������: ' + CAST(COUNT(og.GoodId) AS VARCHAR(10)) + CHAR(13)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id
JOIN Brand b ON g.BrandId = b.Id
GROUP BY b.BrandName

PRINT @BrandsSold

DECLARE @TotalSoldTypes INT
SELECT @TotalSoldTypes = COUNT(DISTINCT g.TypeId)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id

PRINT '����� ����� �������: ' + CAST(@TotalSoldTypes AS VARCHAR(10))

DECLARE @TypesSold NVARCHAR(MAX) = ''

SELECT @TypesSold = @TypesSold + '���: ' + t.TypeName + ' - �������: ' + CAST(COUNT(og.GoodId) AS VARCHAR(10)) + CHAR(13)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id
JOIN Type t ON g.TypeId = t.Id
GROUP BY t.TypeName

PRINT @TypesSold
