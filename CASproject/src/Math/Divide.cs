using CAS;

public class Divide : MathObject {
    public readonly MathObject numerator;
    public readonly MathObject denominator;
    public Divide(MathObject numerator, MathObject denominator) {
        this.numerator = numerator;
        this.denominator = denominator;
    }
    
    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //calculate all terms
        return new Divide(numerator.Evaluate(definedObjects),denominator.Evaluate(definedObjects));
    }

    /*
    Simplifications:
    a/(a^b) = a^(1-b)
    (a^b)/a = a^(b-1)
    (a^b)/(a^c) = a^(b-c)
    */
    public MathObject Simplify() {
        var num = numerator.Simplify();
        var denom = denominator.Simplify();

        //a/a = 1
        if(num.Equals(denom)) return new Constant(1);

        //combine constants
        if(num is Constant num1) {
            if(denom is Constant denom1) return new Constant(num1.value/denom1.value);

            //0/n = 0
            if(num1.value==0) return new Constant(0);
        }

        //a/(b/c) = (a*c)/b
        if(denom is Divide div) return new Divide(new Multiply(num,div.numerator),div.denominator).Simplify();

        //(a/b)/c = a/(b*c)
        if(num is Divide Div) return new Divide(Div.numerator,new Multiply(Div.denominator,denom)).Simplify();

        if(denom is Multiply dmult) {
            //a/(b*a) = 1/b
            if(dmult.terms.FindOtherTerm(n => n.Equals(num),out int dIndex)) {
                dmult.terms.RemoveAt(dIndex);
                if(dmult.terms.Count==1) return new Divide(new Constant(1),dmult.terms[0]);
                return new Divide(new Constant(1),dmult);
            }

            //(a*b)/(a*c)
            if(num is Multiply nmult) {
                var smallest = nmult.terms.Count<dmult.terms.Count? nmult:dmult;
                var biggest = nmult.terms.Count<dmult.terms.Count? dmult:nmult;
                for (int i=0;i<smallest.terms.Count;i++) {
                    var term = smallest.terms[i];
                    if(biggest.terms.FindOtherTerm(n => n.Equals(term),out int termIndex)) {
                        biggest.terms.RemoveAt(termIndex);
                        smallest.terms.RemoveAt(i);
                        i--;
                    }
                }
                return new Divide(nmult,dmult).Simplify();
            }

            //(a*b)/a = b
            if(num is Multiply mult 
            && mult.terms.FindOtherTerm(n => n.Equals(denom),out int index)) {
                    mult.terms.RemoveAt(index);
                    if(mult.terms.Count==1) return mult.terms[0];
                    return mult;
            }
        }

        return new Divide(num,denom);
    }

    public MathObject Differentiate(string variable) {
        if(denominator is Constant num) 
            return new Divide(numerator.Differentiate(variable),num);
        
        //(f/g)' = (f'g - fg')/g^2
        return new Divide(
            new Add(new Multiply(numerator.Differentiate(variable),denominator),Add.Negate(new Multiply(numerator,denominator.Differentiate(variable)))),
            new Multiply(denominator,denominator)
        );
    }

    public bool ContainsAny(MathObject obj) => 
        obj.Equals(this) || 
        numerator.Equals(obj) || denominator.Equals(obj) ||
        numerator.ContainsAny(obj) || denominator.ContainsAny(obj);

    public bool Equals(MathObject obj) =>
        obj is Divide &&                                    //same type
        ((Divide)obj).numerator.Equals(this.numerator) &&   //same terms
        ((Divide)obj).denominator.Equals(this.denominator); //same terms

    public string AsString() => 
        (numerator.Precedence()!=0 && numerator.AbsPrecedence()<Math.Abs(this.Precedence())? "("+numerator.AsString()+")":numerator.AsString()) + "/"+
        (denominator.Precedence()!=0 && denominator.AbsPrecedence()<Math.Abs(this.Precedence())? "("+denominator.AsString()+")":denominator.AsString());
    public int Precedence() => Operator.Precedence('/');
}