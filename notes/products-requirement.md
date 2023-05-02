# Products

## When products are added:

A slug is generated for each product.
    - a unique name based on the product name, stripped of special characters
    - spaces are replaced with dashes
    - the slug has to be unique to all other products
    - if the slug isn't unique, a "uniqueifier" is added to the end in the form of `-a` or `-b` etc. until a unique slug is created.

Pricing
    - Prices are generated from the cost of the product
    - Two prices are generated:
        - Wholesale Price
            - Not all manufacturers of products allow wholesale, so it is the same as the retail price.
            - Wholesale is 7.8% Markup on Cost, with a minimum purchase of:
                - 10 for products that cost less than $10.00
                - 5 for products that are >= $10.00
        - Retail Price
            - The retail price is dependent on the supplier. Some suppliers do not restrict, so it is a 20% markup on the cost.
            - Some retailers have a strict MSRP, so we have to use that.



When adding a new product to the catalog, the purchasing agent has:

```
Name: the name of the product, as it should be displayed in our catalog.
Cost: the current cost of the product from the supplier
Supplier:
    - The ID of the supplier
    - the SKU of the product we are listing for the supplier
```

The created product should contain:

```
Name: the name of the product, as it should be displayed in our catalog.
Slug: The "sluggified name of the product"
Pricing Information:
    - The current pricing for this product.
        - Wholesale (if allowed by manufacturer):
            - wholesale price
            - minimum purchase required for this price
        - Retail price
```



Additionally, for *internal use*, the actual *cost* of each product should be available.



## Notes

### Costs from suppliers sometimes change, and the pricing should reflect that.

Pricing should be versioned. Some customers might be working with older versions of the 
price list, and in certain cases (for big customers) we may give them a grace period.

### The suppliers API is being built by another team. It should be available "real soon now".

Spoiler, they never get it done and we will have to build it. As usual. Slackers.

## Tasks





    