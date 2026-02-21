SELECT
    f.name          as fridge_name,
    p.name          as product_name,
    fp.quantity     as quantity,
    p.default_quantity  as default_quantity
FROM fridge f
JOIN fridge_products fp ON f.id = fp.fridge_id
JOIN products p         ON p.id = fp.product_id
WHERE p.default_quantity IS NOT NULL
    AND fp.quantity > p.default_quantity
ORDER BY f.name, p.name;