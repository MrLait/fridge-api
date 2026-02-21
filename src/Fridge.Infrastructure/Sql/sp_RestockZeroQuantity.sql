CREATE OR ALTER PROCEDURE dbo.sp_RestockZeroQuantity
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        fp.id AS fridge_product_id,
        fp.fridge_id,
        fp.product_id,
        ISNULL(p.default_quantity, 0) AS default_quantity
    FROM fridge_products fp
    INNER JOIN products p ON p.id = fp.product_id
    WHERE fp.quantity = 0;
END
GO