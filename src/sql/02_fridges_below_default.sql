SELECT
    f.name as fridge_name,
    p.name as product_name,
    fp.quantity as quantity,
    p.default_quantity as default_quantity
FROM fridge_products fp 
JOIN fridge f ON fp.fridge_id = f.id
JOIN products p ON fp.product_id = p.id
WHERE p.default_quantity IS NOT NULL
    AND fp.quantity < p.default_quantity
ORDER BY fridge_name, product_name;