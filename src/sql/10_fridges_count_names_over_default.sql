SELECT
    f.name                          as fridge_name,
    COUNT(DISTINCT fp.product_id)   as product_name_cnt
FROM fridge f
JOIN fridge_products fp ON f.id = fp.fridge_id
JOIN products p         ON p.id = fp.product_id
WHERE p.default_quantity IS NOT NULL
    AND fp.quantity > p.default_quantity
GROUP BY f.id, f.name
ORDER BY f.name;