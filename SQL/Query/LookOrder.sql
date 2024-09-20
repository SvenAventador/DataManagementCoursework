SELECT u.UserFio AS "ФИО клиента",
	   u.UserEmail AS "Почта клиента",
	   o.OrderDate AS "Дата заказа",
	   o.OrderPrice AS "Цена заказа",
	   b.BrandName + ' ' + g.NameGood AS "Наименование товара"
FROM [Order] o
JOIN [User] u ON o.UserId = u.Id
JOIN OrderGood og ON o.Id = og.OrderId
JOIN Good g ON og.GoodId = g.Id
JOIN Brand b ON g.BrandId = b.Id
WHERE u.UserRole = 'Пользователь';
