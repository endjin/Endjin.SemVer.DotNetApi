Feature: LibraryComparison
    In order to be able to determine whether a new version of a library follows semantic versioning rules
    As a developer of semantic version checking tools
    I want to be able to discover the changes between two versions of a component

Scenario: Type added
    Given generator 'c1' defines a class named 'Test.Type1'
    And generator 'c2' defines a class named 'Test.Type2'
    And class 'c1' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c2' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c2' has a method named 'Method2' with signature type 'StringReturnStringAndIntParams'
    And class 'c2' has a read/write property named 'Property1' of type 'double'
    And class 'c2' has an event named 'Event1' of type 'System.EventHandler'
    And class 'c2' has a field named 'Field1' of type 'long'
    And the old assembly has the class 'c1'
    And the new assembly has the class 'c1'
    And the new assembly has the class 'c2'
    When I compare the assemblies
    Then the LibraryChanges should report 1 added type
    And the LibraryChanges should report 0 changed types
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesAdded should contain type named 'Test.Type2'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed constructors
    And LibraryChanges.TypesAdded 'Test.Type2' should report 1 added constructors
    And LibraryChanges.TypesAdded 'Test.Type2' should have new constructor matching signature 'VoidNoParams'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed methods
    And LibraryChanges.TypesAdded 'Test.Type2' should report 2 added method
    And LibraryChanges.TypesAdded 'Test.Type2' should have new method 'Method1' matching signature 'VoidNoParams'
    And LibraryChanges.TypesAdded 'Test.Type2' should have new method 'Method2' matching signature 'StringReturnStringAndIntParams'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed properties
    And LibraryChanges.TypesAdded 'Test.Type2' should report 1 added properties
    And LibraryChanges.TypesAdded 'Test.Type2' should have new read/write property 'Property1' of type 'System.Double'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed fields
    And LibraryChanges.TypesAdded 'Test.Type2' should report 1 added fields
    And LibraryChanges.TypesAdded 'Test.Type2' should have new field 'Field1' of type 'System.Int64'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed events
    And LibraryChanges.TypesAdded 'Test.Type2' should report 1 added events
    And LibraryChanges.TypesAdded 'Test.Type2' should have new event 'Event1' of type 'System.EventHandler'

Scenario: Type removed
    Given generator 'c1' defines a class named 'Test.Type1'
    And generator 'c2' defines a class named 'Test.Type2'
    And class 'c1' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c2' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c2' has a method named 'Method2' with signature type 'StringReturnStringAndIntParams'
    And class 'c2' has a read/write property named 'Property1' of type 'double'
    And class 'c2' has an event named 'Event1' of type 'System.EventHandler'
    And class 'c2' has a field named 'Field1' of type 'long'
    And the old assembly has the class 'c1'
    And the old assembly has the class 'c2'
    And the new assembly has the class 'c1'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added type
    And the LibraryChanges should report 0 changed types
    And the LibraryChanges should report 1 removed types
    And LibraryChanges.TypesRemoved should contain type named 'Test.Type2'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 1 removed constructors
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added constructors
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed constructor matching signature 'VoidNoParams'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 2 removed methods
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added method
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed method 'Method1' matching signature 'VoidNoParams'
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed method 'Method2' matching signature 'StringReturnStringAndIntParams'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 1 removed properties
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added properties
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed read/write property 'Property1' of type 'System.Double'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 1 removed fields
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added fields
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed field 'Field1' of type 'System.Int64'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 1 removed events
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added events
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed event 'Event1' of type 'System.EventHandler'

Scenario: Type with non default ctor added
    Given generator 'c1' defines a class named 'Test.Type1'
    And generator 'c2' defines a class named 'Test.Type2'
    And class 'c2' has a constructor with signature type 'VoidReturnStringAndIntParams'
    And the old assembly has the class 'c1'
    And the new assembly has the class 'c1'
    And the new assembly has the class 'c2'
    When I compare the assemblies
    Then the LibraryChanges should report 1 added type
    And the LibraryChanges should report 0 changed types
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesAdded should contain type named 'Test.Type2'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed constructors
    And LibraryChanges.TypesAdded 'Test.Type2' should report 1 added constructors
    And LibraryChanges.TypesAdded 'Test.Type2' should have new constructor matching signature 'VoidReturnStringAndIntParams'
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed methods
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 added method
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed properties
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 added properties
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed fields
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 added fields
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 removed events
    And LibraryChanges.TypesAdded 'Test.Type2' should report 0 added events

Scenario: Type with non default ctor removed
    Given generator 'c1' defines a class named 'Test.Type1'
    And generator 'c2' defines a class named 'Test.Type2'
    And class 'c2' has a constructor with signature type 'VoidReturnStringAndIntParams'
    And the old assembly has the class 'c1'
    And the old assembly has the class 'c2'
    And the new assembly has the class 'c1'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added type
    And the LibraryChanges should report 0 changed types
    And the LibraryChanges should report 1 removed types
    And LibraryChanges.TypesRemoved should contain type named 'Test.Type2'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 1 removed constructors
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added constructors
    And LibraryChanges.TypesRemoved 'Test.Type2' should have removed constructor matching signature 'VoidReturnStringAndIntParams'
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 removed methods
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added method
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 removed properties
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added properties
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 removed fields
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added fields
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 removed events
    And LibraryChanges.TypesRemoved 'Test.Type2' should report 0 added events

Scenario: Constructor added
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1b' has a constructor with signature type 'VoidNoParams'
    And class 'c1b' has a constructor with signature type 'VoidReturnStringAndIntParams'

    # Identical aspects:
    And class 'c1a' has a method named 'MethodUnchanging' with signature type 'VoidNoParams'
    And class 'c1a' has a read/write property named 'PropertyUnchanging' of type 'double'
    And class 'c1a' has an event named 'EventUnchanging' of type 'System.EventHandler'
    And class 'c1a' has a field named 'FieldUnchanging' of type 'long'
    And class 'c1b' has a method named 'MethodUnchanging' with signature type 'VoidNoParams'
    And class 'c1b' has a read/write property named 'PropertyUnchanging' of type 'double'
    And class 'c1b' has an event named 'EventUnchanging' of type 'System.EventHandler'
    And class 'c1b' has a field named 'FieldUnchanging' of type 'long'

    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should have new constructor matching signature 'VoidReturnStringAndIntParams'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

Scenario: Constructor removed
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has a constructor with signature type 'VoidNoParams'
    And class 'c1a' has a constructor with signature type 'VoidReturnStringAndIntParams'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should have removed constructor matching signature 'VoidReturnStringAndIntParams'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

Scenario Outline: Method added
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1b' has a method named 'Method1' with signature type '<signature>'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should have new method 'Method1' matching signature '<signature>'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

    Examples:
    | signature                    |
    | VoidNoParams                 |
    | VoidReturnStringAndIntParams |
    | StringReturnNoParams		   |
    | StringReturnStringAndIntParams |

Scenario Outline: Method removed
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has a method named 'Method1' with signature type '<signature>'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should have removed method 'Method1' matching signature '<signature>'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

    Examples:
    | signature                      |
    | VoidNoParams                   |
    | VoidReturnStringAndIntParams   |
    | StringReturnNoParams           |
    | StringReturnStringAndIntParams |


Scenario Outline: Method overload added
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c1b' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c1b' has a method named 'Method1' with signature type '<signature>'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should have new method 'Method1' matching signature '<signature>'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

    Examples:
    | signature                    |
    | VoidReturnStringParams       |
    | VoidReturnStringAndIntParams |

Scenario Outline: Method overload removed
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c1b' has a method named 'Method1' with signature type 'VoidNoParams'
    And class 'c1a' has a method named 'Method1' with signature type '<signature>'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should have removed method 'Method1' matching signature '<signature>'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

    Examples:
    | signature                    |
    | VoidReturnStringParams       |
    | VoidReturnStringAndIntParams |

Scenario: Property added
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1b' has a read/write property named 'Property1' of type 'double'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should have new read/write property 'Property1' of type 'System.Double'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

Scenario: Property removed
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has a read/write property named 'Property1' of type 'double'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should have removed read/write property 'Property1' of type 'System.Double'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

Scenario: Field added
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1b' has a field named 'Field1' of type 'short'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should have new field 'Field1' of type 'System.Int16'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

Scenario: Field removed
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has a field named 'Field1' of type 'short'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should have removed field 'Field1' of type 'System.Int16'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events

Scenario: Event added
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1b' has an event named 'Event1' of type 'System.EventHandler'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 added events
    And LibraryChanges.TypesChanged 'Test.Type1' should have new event 'Event1' of type 'System.EventHandler'

Scenario: Event removed
    Given generator 'c1a' defines a class named 'Test.Type1'
    And generator 'c1b' defines a class named 'Test.Type1'
    And class 'c1a' has an event named 'Event1' of type 'System.EventHandler'
    And classes 'c1a' and 'c1b' each have some identical methods, properties, events, and fields
    And the old assembly has the class 'c1a'
    And the new assembly has the class 'c1b'
    When I compare the assemblies
    Then the LibraryChanges should report 0 added types
    And the LibraryChanges should report 1 changed type
    And the LibraryChanges should report 0 removed types
    And LibraryChanges.TypesChanged should contain type named 'Test.Type1'
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added constructors
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added methods
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added properties
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 removed fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added fields
    And LibraryChanges.TypesChanged 'Test.Type1' should report 1 removed events
    And LibraryChanges.TypesChanged 'Test.Type1' should report 0 added events
    And LibraryChanges.TypesChanged 'Test.Type1' should have removed event 'Event1' of type 'System.EventHandler'

# Nested types

# Property accessor changes from read/write to read-only
# Parameterized properties (e.g., indexers)
# Add-only events? Are there such things?
# changes to visibility of either get or set, add or remove.