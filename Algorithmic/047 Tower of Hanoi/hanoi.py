import os

towers = [[],[],[]]

# ----------------------

MAX_HEIGHT = 5
WAIT_FOR_INPUT = True
CLEAR_EVERY_STEP = True

# ----------------------

for i in range(MAX_HEIGHT, 0, -1):
    towers[0].append(i)


def hanoi(n, source, target, helper):
    if n > 0:
        hanoi(n-1,source, helper, target)

        target.append(source.pop())
        if WAIT_FOR_INPUT:
            input("Press enter to continue...")
        if CLEAR_EVERY_STEP:
            os.system('cls')

        displayTowers()

        hanoi(n-1, helper, target, source)



def displayTowers():
    for i in range(MAX_HEIGHT-1, -1, -1):
        for tower in towers:
            if i < len(tower):
                ele = tower[i]
                print(" " * (MAX_HEIGHT - ele + 1), end="")
                print(("#" * (2 * ele - 1)), end="")
                print(" " * (MAX_HEIGHT - ele + 1), end="")
            else:
                print(" " * MAX_HEIGHT, end="")
                print("|", end="")
                print(" " * MAX_HEIGHT, end="")
            print("  ", end="")
        print()
    
    for tower in towers:
        print("-" * (2*MAX_HEIGHT+1), end="  ")
    print()

displayTowers()
hanoi(MAX_HEIGHT, towers[0], towers[1], towers[2])