def fizzbuzz(n):
    for i in range(n+1):
        out = ""
        if i % 3 == 0:
            out += "Fizz"
        if i % 5 == 0:
            out += "Buzz"
        if len(out) == 0:
            out += str(i)
        print(out)

fizzbuzz(10)