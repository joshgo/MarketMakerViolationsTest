# MarketMakerViolationsTest

Coding/whiteboard exercise given during interview. 

## Problem

Looking through a firm's market data prices (which is already in time order), 
calculate the number of minutes when the firm was in "violation". A firm is in
"violation", when their price is inferior by more than 10%. 

Examples:

* Best Bid is at $10, and the firm's price is a $7   (violation)
* Best Ask is at $20, and the firm's price is at $24 (violation)
* Best Bid is at $10, and the firm's price is at $9  (Ok)
* Best Ask is at $20, and the firm's price is at $21 (Ok)
* Firm's Ask or Bid is missing ....                  (violation)


## Note to self ...

A truly fun little problem, it's an interesting exercise in dealing with time
and events. Traditionally, in programming we think in discrete values. For 
example loops, those are very specific finite values. With time it is a 
continuous variable, and that's the one thing I missed initially during the 
exercise. And if you're not used to thinking like that, it could be confusing at
first. Also, this was a good exercise in use-cases. The code covered the basic
cases, but the edge cases I did miss. I think the time aspect that through me
for a loop.

Anyway, it was fun! 

## Things to remember:
* Use Cases, especially the edge cases
* Time is continuous