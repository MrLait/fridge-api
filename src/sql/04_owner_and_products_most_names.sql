WITH fridge_by_names_cnt AS
(
    SELECT TOP 1 WITH TIES
        f.id            as fridge_id,
        f.owner_name    as owner_name,
        COUNT(DISTINCT fp.product_id) as product_names_count 
    FROM fridge f
    JOIN fridge_products fp ON f.id = fp.fridge_id
    GROUP BY f.id, f.owner_name
    ORDER BY COUNT(DISTINCT fp.product_id) DESC
)
SELECT
    x.fridge_id             as fridge_id,
    x.owner_name            as owner_name,
    p.name                  as product_name,
    x.product_names_count   as product_names_count
FROM fridge_by_names_cnt x
JOIN fridge_products fp ON fp.fridge_id = x.fridge_id
JOIN products p         ON p.id = fp.product_id
ORDER BY x.fridge_id, p.name;