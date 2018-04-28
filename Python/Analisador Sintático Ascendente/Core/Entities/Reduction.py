from Core.Entities.Production import Production


class Reduction:
    def __init__(self, id, production: Production):
        self.id = id
        self.production = production
    
    def __str__(self):
        return str(self.id)

    def __repr__(self):
        return str(self)
        