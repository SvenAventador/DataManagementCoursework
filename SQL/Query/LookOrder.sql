SELECT u.UserFio AS "��� �������",
	   u.UserEmail AS "����� �������",
	   o.OrderDate AS "���� ������",
	   o.OrderPrice AS "���� ������",
	   b.BrandName + ' ' + g.NameGood AS "������������ ������"
FROM [Order] o
JOIN [User] u ON o.UserId = u.Id
JOIN OrderGood og ON o.Id = og.OrderId
JOIN Good g ON og.GoodId = g.Id
JOIN Brand b ON g.BrandId = b.Id
WHERE u.UserRole = '������������';
