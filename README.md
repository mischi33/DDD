# DDD in practice course
## Requirements
### Snackmachines
- One can only insert one, ten or quarter cents and one, five or twenty dollar bills
- Should only provide snacks when enough money has been inserted
- It is not possible to buy a snack if not enough change is inside the machine or if the machine would have enough money but cannot provide the required denomintaion
- It should be possible to return all inserted money
- The returned money and the change should always use the largest possible unit of money

### ATMs
- It is not possible to withdraw an amount of money less than zero
- One cannot withdraw money if the amount inside the ATM is smaller than the requested amount
- The ATM should refuse a transaction if the denomitation of the requested amount is not possible
- If a withdrawal is performed a commission fee of 0.01% of the amount to be withdrawn is applied
- The commission fee should never be less than 0.01$
- The entirety of all performed transactions (withdrawal amount + fee) should be stored on the ATM

### Management
- Head office keeps track of all user payments and moves cash from snack machines to ATMs
- The money charged from the users bank cards should go to the bank account of the head office
- The transferral of money from a snack machine to an ATM is not performed directly - it first goes to the head office's bank account and only after that to the ATM