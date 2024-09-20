DECLARE @TotalSoldGoods INT
SELECT @TotalSoldGoods = COUNT(DISTINCT og.GoodId)
FROM OrderGood og

PRINT 'Всего товаров продано: ' + CAST(@TotalSoldGoods AS VARCHAR(10))

DECLARE @GoodsSold NVARCHAR(MAX) = ''

SELECT @GoodsSold = @GoodsSold + 'Товар:' + CONCAT(b.BrandName, ' ' , g.NameGood) + ' - Продано: ' + CAST(COUNT(og.GoodId) AS VARCHAR(10)) + CHAR(13)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id
JOIN Brand b ON g.BrandId = b.Id
GROUP BY b.BrandName, g.NameGood

PRINT @GoodsSold
DECLARE @TotalSoldBrands INT
SELECT @TotalSoldBrands = COUNT(DISTINCT g.BrandId)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id

PRINT 'Всего брендов продано: ' + CAST(@TotalSoldBrands AS VARCHAR(10))

DECLARE @BrandsSold NVARCHAR(MAX) = ''

SELECT @BrandsSold = @BrandsSold + 'Бренд: ' + b.BrandName + ' - Продано: ' + CAST(COUNT(og.GoodId) AS VARCHAR(10)) + CHAR(13)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id
JOIN Brand b ON g.BrandId = b.Id
GROUP BY b.BrandName

PRINT @BrandsSold

DECLARE @TotalSoldTypes INT
SELECT @TotalSoldTypes = COUNT(DISTINCT g.TypeId)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id

PRINT 'Всего типов продано: ' + CAST(@TotalSoldTypes AS VARCHAR(10))

DECLARE @TypesSold NVARCHAR(MAX) = ''

SELECT @TypesSold = @TypesSold + 'Тип: ' + t.TypeName + ' - Продано: ' + CAST(COUNT(og.GoodId) AS VARCHAR(10)) + CHAR(13)
FROM OrderGood og
JOIN Good g ON og.GoodId = g.Id
JOIN Type t ON g.TypeId = t.Id
GROUP BY t.TypeName

PRINT @TypesSold
