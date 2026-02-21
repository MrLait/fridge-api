SELECT 'fridge_model'   as [table], COUNT(*) AS record_cnt FROM fridge_model
UNION ALL
SELECT 'fridge'         as [table], COUNT(*) FROM fridge
UNION ALL
SELECT 'fridge_products'as [table], COUNT(*) FROM fridge_products
UNION ALL
SELECT 'products'       as [table], COUNT(*) FROM products;
