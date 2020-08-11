Feature: LibraryChangeTypeDetection
    In order to avoid breaking semantic versioning rules
    As a library author
    I want to be able to determine what type of library change

Scenario: No public changes to library
    Given the library neither adds nor removes anything
    When I compare libaries
    Then the minimum acceptable change type is 'None'

Scenario: Public types added to library
    Given the new library adds a type
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    And whether or not fields are added
    When I compare libaries
    Then the minimum acceptable change type is 'Minor'

Scenario: Public constructors added to existing type in library
    Given the new library adds a constructor
    And whether or not methods are added
    And whether or not types are added
    And whether or not properties are added
    And whether or not fields are added
    When I compare libaries
    Then the minimum acceptable change type is 'Minor'

Scenario: Public methods added to existing type in library
    Given the new library adds a method
    And whether or not types are added
    And whether or not constructors are added
    And whether or not properties are added
    And whether or not fields are added
    When I compare libaries
    Then the minimum acceptable change type is 'Minor'

Scenario: Public properties added to existing type in library
    Given the new library adds a property
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not fields are added
    When I compare libaries
    Then the minimum acceptable change type is 'Minor'

Scenario: Public fields added to existing type in library
    Given the new library adds a field
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    When I compare libaries
    Then the minimum acceptable change type is 'Minor'

Scenario: Public types removed from library
    Given the new library removes a type
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    And whether or not fields are added
    And whether or not constructors are removed
    And whether or not methods are removed
    And whether or not properties are removed
    And whether or not fields are removed
    When I compare libaries
    Then the minimum acceptable change type is 'Major'

Scenario: Public constructors removed from existing type in library
    Given the new library removes a constructor
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    And whether or not fields are added
    And whether or not types are removed
    And whether or not methods are removed
    And whether or not properties are removed
    And whether or not fields are removed
    When I compare libaries
    Then the minimum acceptable change type is 'Major'

Scenario: Public methods removed from existing type in library
    Given the new library removes a method
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    And whether or not fields are added
    And whether or not types are removed
    And whether or not constructors are removed
    And whether or not properties are removed
    And whether or not fields are removed
    When I compare libaries
    Then the minimum acceptable change type is 'Major'

Scenario: Public properties removed from existing type in library
    Given the new library removes a property
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    And whether or not fields are added
    And whether or not types are removed
    And whether or not constructors are removed
    And whether or not methods are removed
    And whether or not fields are removed
    When I compare libaries
    Then the minimum acceptable change type is 'Major'

Scenario: Public fields removed from existing type in library
    Given the new library removes a field
    And whether or not types are added
    And whether or not constructors are added
    And whether or not methods are added
    And whether or not properties are added
    And whether or not fields are added
    And whether or not types are removed
    And whether or not constructors are removed
    And whether or not methods are removed
    And whether or not properties are removed
    When I compare libaries
    Then the minimum acceptable change type is 'Major'

#Scenario: Public nested types removed from library
#	Given the new library is missing a type
#	When I compare libaries
#	Then the minimum acceptable change type is 'Major'
#
#Scenario: Public protected types removed from library
#	Given the new library is missing a type
#	When I compare libaries
#	Then the minimum acceptable change type is 'Major'
#
#
## TODO:
## higher-level changes, such as available libraries, and supported frameworks
# What about things like changes of visibility: from public to protected of vice versa?