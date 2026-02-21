SELECT 
    f.name as fridge_name,
    fm.name as model_name,
    p.name as product_name,
    fp.quantity as product_quantity
FROM fridge f
JOIN fridge_model fm    ON fm.id = f.model_id
JOIN fridge_products fp ON f.id = fp.fridge_id
JOIN products p         ON p.id = fp.product_id
WHERE fm.name LIKE 'A%'
ORDER BY fridge_name, product_name;