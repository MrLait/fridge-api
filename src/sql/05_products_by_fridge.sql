SELECT
    fp.fridge_id    as fridge_id,
    p.name          as product_name
FROM fridge_products fp
JOIN products p             On p.id = fp.product_id
WHERE fp.fridge_id = '1549EEC3-E01B-4A17-B6E4-3726B90EF3CD'
ORDER BY p.name;