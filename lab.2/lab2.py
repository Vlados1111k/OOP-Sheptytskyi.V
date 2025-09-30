class BankAccount:
    def __init__(self, balance=0):
        self._balance = balance if balance >= 0 else 0  

    @property
    def balance(self):
        """Геттер для балансу"""
        return self._balance

    @balance.setter
    def balance(self, value):
        """Сеттер з валідацією (баланс >= 0)"""
        if value < 0:
            raise ValueError("Баланс не може бути від’ємним!")
        self._balance = value

    def __add__(self, amount):
        """Перевантаження оператора + (поповнення)"""
        if not isinstance(amount, (int, float)):
            return NotImplemented
        return BankAccount(self._balance + amount)

    def __sub__(self, amount):
        """Перевантаження оператора - (зняття)"""
        if not isinstance(amount, (int, float)):
            return NotImplemented
        if self._balance - amount < 0:
            raise ValueError("Недостатньо коштів на рахунку!")
        return BankAccount(self._balance - amount)

    def __str__(self):
        return f"BankAccount(balance={self._balance})"


if __name__ == "__main__":
    acc1 = BankAccount(100)
    print(acc1)  

    acc2 = acc1 + 50 
    print(acc2)  

    acc3 = acc2 - 70  
    print(acc3) 

