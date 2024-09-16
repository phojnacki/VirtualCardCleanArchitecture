# VirtualCardCleanArchitecture

The business goal is to implement a virtual credit card (credit line). The business system is supposed to simulate holding a credit card without the actual production of plastic or generating the card number. This helps avoid certain inconvenient regulations ;)

Opening a virtual credit card leads to its automatic activation. Such a card has its predefined limit in a given currency. The virtual card is identified by a unique contract number.

The current offer assumes that a person can only hold one virtual card. The system must, of course, have the ability to connect the card with its holder and display their PESEL number, first name, and last name. The card can be shared with another person (a friend or relative).

An active card is ready to use. Using the card consists of a series of chronological settlement cycles, each lasting 30 days. Using the card involves specifying the amount to withdraw. The amount can be withdrawn if it fits within the limit and if the card is active. The card must know its limit and the available limit (usable amount).

An additional condition is the maximum number of withdrawals in a given cycle, which is limited to 45.

Withdrawals involve an automatic calculation of a fee as a percentage of the withdrawal amount. For example, a withdrawal of 100 PLN with a fee of 15% will calculate a fee of 15 PLN. The system must aggregate the list of withdrawals made on the card and distinguish which withdrawals occurred in the given cycle. The card must also know its calculated fees. After a withdrawal, BIK is informed, and an email is sent upon payment.

At the end of the cycle (at night, 31 days from the opening date), a statement is generated with the total amount due (used limit, possible interest, fees if implemented).

Unpaid debt results in the deactivation of the card.

The withdrawal process is crucial in terms of efficiency and must be scalable. Withdrawing more than is on the business account could be a financial disaster. The inability to withdraw the full available amount could harm the company's reputation.