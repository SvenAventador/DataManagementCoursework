SELECT s.ShopName as "�������� ��������",
	   CONCAT(b.BrandName, ' ', g.NameGood) as "������������ ������",
	   t.TypeName as "��� ������",
	   g.GoodPrice as "���� ������",
	   g.GoodCount as "���������� ������ �� ������"
FROM Shop s
JOIN ShopGood sg ON s.Id = sg.shopId
JOIN Good g ON sg.GoodId = g.Id
JOIN [Type] t ON g.typeId = t.Id
JOIN Brand b ON g.brandId = b.Id
