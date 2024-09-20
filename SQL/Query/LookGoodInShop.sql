SELECT s.ShopName as "Название магазина",
	   CONCAT(b.BrandName, ' ', g.NameGood) as "Наименование товара",
	   t.TypeName as "Тип товара",
	   g.GoodPrice as "Цена товара",
	   g.GoodCount as "Количество товара на складе"
FROM Shop s
JOIN ShopGood sg ON s.Id = sg.shopId
JOIN Good g ON sg.GoodId = g.Id
JOIN [Type] t ON g.typeId = t.Id
JOIN Brand b ON g.brandId = b.Id
