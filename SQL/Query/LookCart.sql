DECLARE @CartId  INT
SET @CartId = 1

SELECT u.UserFio as "ФИО Заказчика",
	   u.UserEmail as "Почта заказчика",
       CONCAT(b.BrandName, ' ', g.NameGood) AS "Наименование продукта",
       t.TypeName as "Тип продукта",
	   cg.GoodCount as "Количество товара в корзине"
FROM CartGood cg
JOIN Cart c ON cg.CartId = c.Id
JOIN Good g ON cg.GoodId = g.Id
JOIN [User] u ON c.UserId = u.Id
JOIN Brand b ON g.BrandId = b.Id
JOIN Type t ON g.TypeId = t.Id
WHERE c.Id = @CartId
