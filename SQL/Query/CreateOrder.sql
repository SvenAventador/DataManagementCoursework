DECLARE @UserId INT;
DECLARE @OrderId INT;
DECLARE @OrderPrice DECIMAL(18, 2);

SELECT TOP 1 @UserId = UserId 
FROM CartGood cg
JOIN Cart c ON cg.CartId = c.Id;

SELECT @OrderPrice = SUM(g.GoodPrice * cg.GoodCount)
FROM CartGood cg
JOIN Good g ON cg.GoodId = g.Id
WHERE cg.CartId = (SELECT Id FROM Cart WHERE UserId = @UserId);

INSERT INTO [Order] (OrderDate, OrderPrice, UserId)
VALUES (GETDATE(), @OrderPrice, @UserId);

SET @OrderId = SCOPE_IDENTITY();

INSERT INTO OrderGood (GoodId, OrderId)
SELECT cg.GoodId, @OrderId
FROM CartGood cg
WHERE cg.CartId = (SELECT Id FROM Cart WHERE UserId = @UserId);

UPDATE g
SET g.GoodCount = g.GoodCount - cg.GoodCount
FROM Good g
JOIN CartGood cg ON g.Id = cg.GoodId
WHERE cg.CartId = (SELECT Id FROM Cart WHERE UserId = @UserId);

DELETE FROM CartGood 
WHERE CartId = (SELECT Id FROM Cart WHERE UserId = @UserId);

PRINT 'Заказ успешно создан.';
